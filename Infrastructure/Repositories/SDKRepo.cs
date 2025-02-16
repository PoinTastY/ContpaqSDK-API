using Domain.SDK_Comercial;
using System.Text;
using Core.Domain.Entities.SDK.Estructuras;
using Core.Domain.Interfaces.Services;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Exceptions;
using Core.Domain.Entities.DTOs;

namespace Infrastructure.Repositories
{
    public class SDKRepo : ISDKRepo
    {
        private string _nombrePAQ;
        private string _dirEmpresas;
        private string _empresaDefault;
        private string _user;
        private string _password;
        private string _dirBinarios;
        private bool _transactionInProgress;

        private readonly ILogger _logger;

        public SDKRepo(SDKSettings sDKSettings, ILogger logger)
        {
            _nombrePAQ = sDKSettings.NombrePAQ;
            _dirEmpresas = sDKSettings.RutaEmpresas;
            _empresaDefault = sDKSettings.EmpresaDefault;
            _user = sDKSettings.User;
            _password = sDKSettings.Password;
            _dirBinarios = sDKSettings.RutaBinarios;
            _transactionInProgress = false;
            _logger = logger;
        }

        #region SDK General Methods

        public async Task InicializarSDKAsync()
        {
            _logger.Log("Iniciando la inicialización del SDK.");

            // Mueve la inicialización a una tarea
            await Task.Run(() => InicializarSDK());

            _logger.Log("Inicializacion del SDK Exitosa, esperando instrucciones");
        }

        public void InicializarSDK()
        {
            
            SDK.SetCurrentDirectory(_dirBinarios);
            SDK.SetDllDirectory(_dirBinarios);

            _logger.Log("Directorios de aplicacion Establecidos");

            var attempts = 0;
            int lError;

            // Verifica si la DLL existe en el directorio actual
            string dllPath = Path.Combine(_dirBinarios, "MGWServicios.dll");
            if (!File.Exists(dllPath))
            {
                throw new SDKException("No se encontro MGWServicios.dll en el directorio especificado.");
            }

            _logger.Log("DLL encontrada en el directorio especificado.");
            _logger.Log("Intentando Iniciar sesion en SDK...");
            try
            {
                SDK.fInicioSesionSDK(_user, _password);
                _logger.Log("Inicio de sesion exitoso.");
            }
            catch (Exception ex)
            {
                throw new SDKException("No se pudo iniciar sesion en el SDK: " + ex);
            }

            var directory = Directory.GetCurrentDirectory();
            _logger.Log($"Intentando Setear el nombre del PAQ (directorio actual: {directory})...");

            //indicar con que sistema se va a trabajar
            lError = SDK.fSetNombrePAQ(_nombrePAQ);
            _logger.Log($"Resultado de fSetNombrePAQ: {lError.ToString()}");
            if (lError != 0)
            {
                throw new SDKException($"Error al establecer el nombrePAQ ({lError}): ", lError);
            }
            else
            {
                _logger.Log($"NombrePAQ: {_nombrePAQ} establecido con exito.");
            }
                   
            _logger.Log($"Intentando abrir la empresa ({_empresaDefault})...");
            attempts = 0;
            while (true)
            {
                lError = SDK.fAbreEmpresa(_dirEmpresas + _empresaDefault);
                if (lError != 0)
                {
                    if (++attempts > 4)
                    {
                        throw new SDKException($"No se pudo abrir la empresa: {_empresaDefault}, ", lError);
                    }
                    else //retry
                        Thread.Sleep(1000);
                }
                else
                {
                    _logger.Log($"Empresa: {_empresaDefault} abierta con exito.");
                    SDK.fCierraEmpresa();
                    return;
                }
            }
        }

        public void CierraEmpresaSDK()
        {
            try
            {
                 SDK.fCierraEmpresa();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task TerminaSDK()
        {
            await Task.Run(() =>
            {
                if (_transactionInProgress)
                    SDK.fCierraEmpresa();
                SDK.fTerminaSDK();
            });
        }

        public async Task<bool> StartTransaction(string empresa)
        {
            if (_transactionInProgress)
                return false;
            int attempts = 0;
            int lError;
            return await Task.Run(() =>
            {
                while (true)
                {

                    lError = SDK.fAbreEmpresa(_dirEmpresas + empresa);
                    if (lError != 0)
                    {
                        Thread.Sleep(500);
                        if (++attempts > 4)
                        {
                            throw new SDKException($"No se pudo abrir la empresa: {empresa}, Directortio actual: {Directory.GetCurrentDirectory()} ({lError}): ", lError);
                        }
                        Thread.Sleep(500);
                    }
                    else
                    {
                        _transactionInProgress = true;
                        _logger.Log($"Empresa: {empresa} abierta con exito, transaccion iniciada");
                        return true;
                    }
                }
            });
        }

        public void StopTransaction()
        {
            if (_transactionInProgress)
            {
                _transactionInProgress = false;
                SDK.fCierraEmpresa();
                _logger.Log("Transacción finalizada con éxito.");
            }
        }

        #endregion

        #region Document Methods

        public async Task<Dictionary<int, double>> AddDocumento(DocumentoDto documentoDto)
        {
            int idDocumento = 0;
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }

            return await Task.Run(() =>
            {
                tDocumento tDocumento = documentoDto.ToSDKDocumento();

                //declaracion de variables, para obtener el folio por referencia
                double folio = 0;
                StringBuilder serie = new StringBuilder(documentoDto.Serie);

                int lError = SDK.fSiguienteFolio(tDocumento.aCodConcepto, serie, ref folio);
                if (lError != 0)
                {
                    throw new SDKException($"Problema obteniendo el siguiente folio. Concepto: {tDocumento.aCodConcepto}, Serie: {tDocumento.aSerie}: ", lError);
                }

                lError = SDK.fAltaDocumento(ref idDocumento, ref tDocumento);
                if (lError != 0)
                {
                    throw new SDKException($"Error dando de alta el documento: ", lError);
                }
                
                return new Dictionary<int, double> { 
                    { idDocumento, folio } 
                };
            });
        }

        public async Task SetImpreso(int idDocumento, bool impressed)
        {
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }

            await Task.Run(() =>
            {
                if (idDocumento <= 0)
                {
                    throw new SDKException($"Se recibio un id de documento invalido({idDocumento}) para establecer como impreso.");
                }

                int lError = SDK.fBuscarIdDocumento(idDocumento);
                if (lError != 0)
                {
                    throw new SDKException("Error buscando el id del documento: ", lError);

                }

                lError = SDK.fEditarDocumento();
                if (lError != 0)
                {
                    throw new SDKException("Error cambiando a estado de edicion de el documento: ", lError);
                }

                lError = SDK.fDocumentoImpreso(impressed);
                if (lError != 0)
                {
                    throw new SDKException("Hubo un error estableciendo el estado del documento a impreso: ", lError);

                }

                lError = SDK.fGuardaDocumento();
                if (lError != 0)
                {
                    throw new SDKException("Error guardando los cambios en el documento: ", lError);
                }
            });
        }

        public async Task SetDatoDocumento(Dictionary<string, string> camposValores, int idDocumento)
        {
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            
            await Task.Run(() =>
            {
                int lError = SDK.fBuscarIdDocumento(idDocumento);
                if (lError != 0)
                {
                    throw new SDKException($"Error estableciendo el valores en el documento con id: {idDocumento}: ", lError);
                }

                lError = SDK.fEditarDocumento();
                if (lError != 0)
                {
                    throw new SDKException($"Error Cambiando estado a fEditarDocumento: ", lError);
                }

                foreach (var columna in camposValores.Keys)
                {
                    lError = SDK.fSetDatoDocumento(columna, camposValores[columna]);
                    if (lError != 0)
                    {
                        int error = SDK.fCancelarModificacionDocumento();
                        if (error != 0)
                        {
                            throw new SDKException($"Hubo un error ejecutando fSetDatoDocumento ({SDK.rError(lError)}), y despues se intento cancelar la modificacion, lo que resulto en: ", error);
                        }
                        throw new SDKException($"Error estableciendo el valor: {camposValores[columna]} en la columna: {columna} para el documento: {idDocumento}: ", lError);
                    }
                }

                lError = SDK.fGuardaDocumento();
                if (lError != 0)
                {
                    int error = SDK.fCancelarModificacionDocumento();
                    if (error != 0)
                    {
                        throw new SDKException($"Hubo un error ejecutando fGuardaDocumento ({SDK.rError(lError)}), y despues se intento cancelar la modificacion, lo que resulto en: )", error);
                    }
                    throw new SDKException($"Error guardando los cambios previamente establecidos en fGuardaDocumento: ", lError);
                }

            });
        }

        #endregion

        #region Movimiento Methods

        public async Task<int> AddMovimiento(MovimientoDto movimientoDto, int idDocumento)
        {
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            
            return await Task.Run(() =>
            {
                tMovimiento movimiento = movimientoDto.ToSDKMovimiento();

                int idMovimiento = 0;

                int lError = SDK.fAltaMovimiento(idDocumento, ref idMovimiento, ref movimiento);
                if (lError != 0)
                {
                    throw new SDKException($"Error Dando de alta el movimiento: ", lError);
                }

                return idMovimiento;
            });
        }

        #endregion

        #region Producto Methods



        #endregion
    }
}
