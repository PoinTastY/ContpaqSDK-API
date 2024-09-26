using Domain.Exceptions;
using Domain.Entities.Estructuras;
using Domain.Interfaces;
using Domain.SDK_Comercial;
using System.Text;

namespace Infrastructure.Repositories
{
    public class SDKRepo : ISDKRepo
    {
        private string _nombrePAQ;
        private string _dirEmpresa;
        private string _user;
        private string _password;
        private string _dirBinarios;
        private bool _transactionInProgress;

        private readonly SDKSettings _settings;
        private readonly ILogger _logger;

        public SDKRepo(SDKSettings settings, ILogger logger)
        {
            _nombrePAQ = settings.NombrePAQ;
            _dirEmpresa = settings.RutaEmpresa;
            _user = settings.User;
            _password = settings.Password;
            _dirBinarios = settings.RutaBinarios;
            _settings = settings;
            _transactionInProgress = false;
            _logger = logger;

        }

        public async Task InitializeAsync()
        {
            try
            {
                _logger.Log("Iniciando la inicialización del SDK.");

                // Mueve la inicialización a una tarea
                await Task.Run(() => Initialize());

                _logger.Log("Inicializacion del SDK Exitosa, listo pa chambear");
            }
            catch (Exception e)
            {
                _logger.Log($"Error al inicializar el SDK: {e.Message}");
                throw;
            }
        }

        public void Initialize()
        {
            try
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
                while (true)
                {
                    try
                    {
                        lError = SDK.fSetNombrePAQ(_nombrePAQ);
                        _logger.Log("Resultado de intento de fSetNombrePAQ: " + lError);
                        if (lError != 0)
                        {

                            _logger.Log($"Error al establecer el nombrePAQ: {SDK.rError(lError)}");
                            System.Threading.Thread.Sleep(2000);
                            if (++attempts > 5)
                            {
                                throw new SDKException($"Despues de {attempts} intentos, no se pudo establecer el nombrePAQ: ", lError);
                            }

                        }
                        else
                        {
                            _logger.Log($"NombrePAQ: {_nombrePAQ} establecido con exito.");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new SDKException("Error al establecer el nombrePAQ: " + ex);
                    }
                }
                _logger.Log("Intentando abrir la empresa...");
                attempts = 0;
                while (true)
                {
                    lError = SDK.fAbreEmpresa(_dirEmpresa);
                    _logger.Log($"Resultado de intento de fAbreEmpresa: {lError}");
                    if (lError != 0)
                    {
                        if (++attempts > 4)
                        {
                            throw new SDKException($"No se pudo abrir la empresa: {_dirEmpresa}, ", lError);
                        }
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        _logger.Log($"Empresa: {_dirEmpresa} abierta con exito.");
                        SDK.fCierraEmpresa();
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Log($"Error al inicializar el SDK: {e.Message}");
                throw;
            }
        }
        public SDKSettings GetSDKSettings()
        {
            return _settings;
        }

        public string GetBinariesDir()
        {
            return _dirBinarios;
        }

        public void ReleaseSDK()
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


        
        public void DisposeSDK()
        {
            try
            {
                if(_transactionInProgress)
                    SDK.fCierraEmpresa();
                SDK.fTerminaSDK();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> StartTransaction()
        {
            if (_transactionInProgress)
                return false;
            int attempts = 0;
            int lError;
            try
            {
                return await Task.Run(() =>
                {
                    while (true)
                    {

                        lError = SDK.fAbreEmpresa(_dirEmpresa);
                        if (lError != 0)
                        {
                            Thread.Sleep(500);
                            if (++attempts > 4)
                            {
                                throw new SDKException($"No se pudo abrir la empresa: {_dirEmpresa}, Directortio actual: {Directory.GetCurrentDirectory()} ({lError}): ", lError);
                            }
                            Thread.Sleep(500);
                        }
                        else
                        {
                            _transactionInProgress = true;
                            _logger.Log($"Empresa: {_dirEmpresa} abierta con exito.");
                            return true;
                        }
                    }
                });
            }
            catch(Exception) { throw; }
        }

        public void StopTransaction()
        {
            if (_transactionInProgress)
            {
                _transactionInProgress = false;
                SDK.fCierraEmpresa();
            }
        }

        public async Task<Dictionary<int, Double>> AddDocumentWithMovement(tDocumento documento, tMovimiento movimiento)
        {
            int idDocumento = 0;
            if(!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            try
            {
                var idAndDocumento = await AddDocument(documento);
                idDocumento = idAndDocumento.Keys.First();
                try
                {
                    _logger.Log($"Documento agregado con éxito. ID: {idDocumento}, continuando con Movimiento...");
                    var idMovimiento = await AddMovimiento(movimiento, idDocumento);
                    _logger.Log($"Movimiento agregado con éxito. ID: {idMovimiento}");

                }
                catch(Exception ex)
                {
                    throw new Exception($"Se agrego el documento, pero hubo un problema creando el movimiento: {ex.Message}");
                }

                return idAndDocumento;
            }
            catch { throw; }
        }

        public async Task SetDatoDocumento(Dictionary<string, string> camposValores, int idDocumento)
        {
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            int lError = 0;
            try
            {
                await Task.Run(() =>
                {
                    lError = SDK.fBuscarIdDocumento(idDocumento);
                    if (lError != 0)
                    {
                        throw new SDKException($"Error estableciendo el valores en el documento con id: {idDocumento}: ", lError);
                    }

                    lError = SDK.fEditarDocumento();
                    if (lError != 0)
                    {
                        throw new SDKException($"Error Cambiando estado a fEditarDocumento: ", lError);
                    }

                    foreach (var campo in camposValores.Keys)
                    {
                        lError = SDK.fSetDatoDocumento(campo, camposValores[campo]);
                        if (lError != 0)
                        {
                            int error = SDK.fCancelarModificacionDocumento();
                            if (error != 0)
                            {
                                throw new SDKException($"Hubo un error intentando cancelar la modificacion de un Documento: {SDK.rError(error)}, que previamente se intento setear: ", lError);
                            }
                            throw new SDKException($"Error estableciendo el valor: {camposValores[campo]} en el campo: {campo} en el documento: {idDocumento}: ", lError);
                        }
                    }

                    lError = SDK.fGuardaDocumento();
                    if (lError != 0)
                    {
                        int error = SDK.fCancelarModificacionDocumento();
                        if (error != 0)
                        {
                            throw new SDKException($"Hubo un error intentando cancelar la modificacion de un Documento: {SDK.rError(error)}, que previamente se intento setear: ", lError);
                        }
                        throw new SDKException($"Error guardando los cambios previamente establecidos en fGuardaDocumento: ", lError);
                    }

                });
            }
            catch { throw; }
        }

        public async Task<Dictionary<int, Double>> AddDocument(tDocumento documento)
        {
            int lError = 0;
            int idDocumento = 0;
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            try
            {
                return await Task.Run(() =>
                {

                    double folio = 0;
                    StringBuilder serie = new StringBuilder(documento.aSerie);

                    _logger.Log($"Obteniendo el siguiente folio para el documento: {documento.aCodConcepto}, Serie: {serie}");

                    lError = SDK.fSiguienteFolio(documento.aCodConcepto, serie, ref folio);
                    if (lError != 0)
                    {
                        _logger.Log($"Error obteniendo el siguiente folio para el documento: {documento.aCodConcepto}, Serie: {serie}");
                        throw new SDKException($"Problema obteniendo el siguiente folio. Concepto: {documento.aCodConcepto}, Serie: {documento.aSerie}: ", lError);
                    }
                    _logger.Log($"Folio obtenido: {folio}");
                    lError = SDK.fAltaDocumento(ref idDocumento, ref documento);
                    if (lError != 0)
                    {
                        throw new SDKException($"Error dando de alta el documento: ", lError);
                    }
                    else
                    {
                        _logger.Log($"Documento dado de alta con exito. ID: {idDocumento}");
                        return new Dictionary<int, double> { { idDocumento, folio } };
                    }
                });

            }
            catch(Exception) { throw; }
        }

        public async Task<int> AddMovimiento(tMovimiento movimiento, int idDocumento)
        {
            int idMovimiento = 0;
            int lError = 0;
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            try
            {
                await Task.Run(() =>
                {
                    lError = SDK.fAltaMovimiento(idDocumento, ref idMovimiento, ref movimiento);
                    if (lError != 0)
                    {
                        throw new SDKException($"Error Dando de alta el movimiento: ", lError);
                    }
                });
                return idDocumento;
            }
            catch { throw; }
        }


        public async Task SetImpreso(int idDocumento, bool impressed)
        {
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            try
            {
                await Task.Run(() =>
                {
                    if (idDocumento <= 0)
                    {
                        throw new SDKException($"Se recibio un id Invalido@({idDocumento}) para establecer como impreso.");
                    }

                    int lError = SDK.fBuscarIdDocumento(idDocumento);
                    if (lError != 0)
                    {
                        throw new SDKException("Error buscando el id del documento: ", lError);

                    }

                    lError = SDK.fDocumentoImpreso(impressed);
                    if (lError != 0)
                    {
                        throw new SDKException("Hubo un error estableciendo el estado del documento a impreso: ", lError);

                    }
                });
            }
            catch { throw; }
        }
    }
}
