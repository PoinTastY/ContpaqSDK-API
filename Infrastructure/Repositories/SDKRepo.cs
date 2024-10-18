using Domain.Exceptions;
using Domain.Entities.Estructuras;
using Domain.Interfaces;
using Domain.SDK_Comercial;
using System.Text;
using Domain.Entities;
using System.Globalization;
using Domain.Interfaces.Services;

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

        #region SDK General Methods
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
            catch (AccessViolationException e)
            {
                _logger.Log($"Error al inicializar el SDK: {e.Message}");
                throw;
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


        
        public async Task DisposeSDK()
        {
            try
            {
                await Task.Run(() =>
                {
                    if (_transactionInProgress)
                        SDK.fCierraEmpresa();
                    SDK.fTerminaSDK();
                });
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
                            _logger.Log($"Empresa: {_dirEmpresa} abierta con exito, transaccion iniciada");
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
                _logger.Log("Transacción finalizada con éxito.");
            }
        }

        #endregion

        #region Document Methods



            public async Task<DocumentSQL> GetDocumentoById(int idDocumento)
        {
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            if (idDocumento == 0)
            {
                throw new SDKException("No se puede buscar un documento con id 0.");
            }
            try
            {
                return await Task.Run(() =>
                {
                    _logger.Log($"Buscando documento con id: {idDocumento}");
                    int lError = SDK.fBuscarIdDocumento(idDocumento);
                    if (lError != 0)
                    {
                        throw new SDKException($"Error buscando el documento con id: {idDocumento}: ", lError);
                    }

                    var documento = LeeDatosDocumento();
                    return documento;
                });
            }
            catch { throw; }
        }

        public async Task<DocumentSQL> GetDocumentoByConceptoFolioAndSerie(string codConcepto, string serie, string folio)
        {
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            try
            {
                return await Task.Run(() =>
                {
                    _logger.Log($"Buscando documento con folio: {folio}, Serie: {serie}");
                    int lError = SDK.fBuscarDocumento(codConcepto, serie, folio);
                    if (lError != 0)
                    {
                        throw new SDKException($"Error buscando el documento con folio: {folio}, Serie: {serie}: ", lError);
                    }

                    var documento = LeeDatosDocumento();
                    return documento;
                });
            }
            catch { throw; }
        }

        public async Task<DocumentSQL> AddDocumentAndMovements(tDocumento documento, List<tMovimiento> movimientos)
        {
            int idDocumento = 0;
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }

            try
            {
                var documentSQL = await AddDocument(documento);
                try
                {
                    if(documentSQL.CIDDOCUMENTO == 0)
                    {
                        throw new SDKException("No se pudo agregar el documento, por lo que no se pueden agregar los movimientos.");
                    }
                    foreach (var movimiento in movimientos)
                    {
                        await AddMovimiento(movimiento, idDocumento);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Se agrego el documento, pero hubo un problema creando los movimientos: {ex.Message}");
                }
                return documentSQL;
            }
            catch { throw; }
        }

        public async Task<DocumentSQL> AddDocumentWithMovement(tDocumento documento, tMovimiento movimiento)
        {
            int idDocumento = 0;
            if(!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            try
            {
                var documentSQL = await AddDocument(documento);
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

                return documentSQL;
            }
            catch { throw; }
        }

        public async Task<DocumentSQL> AddDocument(tDocumento documento)
        {
            int lError = 0;
            int idDocumento = 0;
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento sin una transacción activa.");
            }

            return await Task.Run(() =>
            {

                double folio = 0;
                StringBuilder serie = new StringBuilder(documento.aSerie);

                lError = SDK.fSiguienteFolio(documento.aCodConcepto, serie, ref folio);
                if (lError != 0)
                {
                    throw new SDKException($"Problema obteniendo el siguiente folio. Concepto: {documento.aCodConcepto}, Serie: {documento.aSerie}: ", lError);
                }
                    
                lError = SDK.fAltaDocumento(ref idDocumento, ref documento);
                if (lError != 0)
                {
                    throw new SDKException($"Error dando de alta el documento: ", lError);
                }
                else
                {
                    var documentSQL = new DocumentSQL();
                    documentSQL.CIDDOCUMENTO = idDocumento;
                    documentSQL.CFOLIO = folio;

                    return documentSQL;
                }
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

        private DocumentSQL LeeDatosDocumento()
        {
            var documento = new DocumentSQL();
            var valor = new StringBuilder(Constantes.kLongCodigo);
            var lError = SDK.fLeeDatoDocumento("CFOLIO", valor, Constantes.kLongitudFolio);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el folio del documento: ", lError);
            }
            documento.CFOLIO = double.Parse(valor.ToString());

            valor = new StringBuilder(Constantes.kLongCodigo);
            lError = SDK.fLeeDatoDocumento("CTOTAL", valor, Constantes.kLongitudMonto);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el total del documento: ", lError);
            }
            documento.CTOTAL = double.Parse(valor.ToString());

            valor = new StringBuilder(Constantes.kLongReferencia);
            lError = SDK.fLeeDatoDocumento("CREFERENCIA", valor, Constantes.kLongReferencia);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo la referencia del documento: ", lError);
            }
            documento.CREFERENCIA = valor.ToString();

            valor = new StringBuilder(Constantes.kLongFecha);
            lError = SDK.fLeeDatoDocumento("CFECHA", valor, Constantes.kLongFecha);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo la fecha del documento: ", lError);
            }
            string fechaLeida = valor.ToString().Substring(0, 10);
            documento.CFECHA = DateTime.ParseExact(fechaLeida, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            valor = new StringBuilder(Constantes.kLongSerie);
            lError = SDK.fLeeDatoDocumento("CSERIEDOCUMENTO", valor, Constantes.kLongSerie);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo la serie del documento: ", lError);
            }
            documento.CSERIEDOCUMENTO = valor.ToString();

            valor = new StringBuilder(Constantes.kLongCodigo);
            lError = SDK.fLeeDatoDocumento("CIDDOCUMENTO", valor, Constantes.kLongCodigo);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el id del documento: ", lError);
            }
            documento.CIDDOCUMENTO = int.Parse(valor.ToString());

            valor = new StringBuilder(Constantes.kLongCodigo);
            lError = SDK.fLeeDatoDocumento("CRAZONSOCIAL", valor, Constantes.kLongCodigo);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo la razon social del documento: ", lError);
            }
            documento.CRAZONSOCIAL = valor.ToString();

            valor = new StringBuilder(Constantes.kLongCodigo);
            lError = SDK.fLeeDatoDocumento("CIMPRESO", valor, Constantes.kLongCodigo);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el estado de impresion del documento: ", lError);
            }
            documento.CIMPRESO = int.Parse(valor.ToString());

            valor = new StringBuilder(Constantes.kLongTextoExtra);
            lError = SDK.fLeeDatoDocumento("COBSERVACIONES", valor, Constantes.kLongTextoExtra);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo las observaciones del documento: ", lError);
            }
            documento.COBSERVACIONES = valor.ToString();

            valor = new StringBuilder(Constantes.kLongTextoExtra);
            lError = SDK.fLeeDatoDocumento("CTEXTOEXTRA1", valor, Constantes.kLongTextoExtra);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el texto extra 1 del documento: ", lError);
            }
            documento.CTEXTOEXTRA1 = valor.ToString();

            valor = new StringBuilder(Constantes.kLongTextoExtra);
            lError = SDK.fLeeDatoDocumento("CTEXTOEXTRA2", valor, Constantes.kLongTextoExtra);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el texto extra 2 del documento: ", lError);
            }
            documento.CTEXTOEXTRA2 = valor.ToString();

            valor = new StringBuilder(Constantes.kLongTextoExtra);
            lError = SDK.fLeeDatoDocumento("CTEXTOEXTRA3", valor, Constantes.kLongTextoExtra);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el texto extra 3 del documento: ", lError);
            }
            documento.CTEXTOEXTRA3 = valor.ToString();

            valor = new StringBuilder(Constantes.kLongCodigo);
            lError = SDK.fLeeDatoDocumento("CIDCLIENTEPROVEEDOR", valor, Constantes.kLongCodigo);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el id del cliente proveedor del documento: ", lError);
            }
            documento.CIDCLIENTEPROVEEDOR = int.Parse(valor.ToString());

            valor = new StringBuilder(Constantes.kLongCodigo);
            lError = SDK.fLeeDatoDocumento("CIDCONCEPTODOCUMENTO", valor, Constantes.kLongCodigo);
            if (lError != 0)
            {
                throw new SDKException("Error leyendo el id del concepto del documento: ", lError);
            }
            documento.CIDCONCEPTODOCUMENTO = int.Parse(valor.ToString());


            return documento;
        }

        #endregion

        #region Movimiento Methods
        public async Task<int> AddMovimiento(tMovimiento movimiento, int idDocumento)
        {
            int idMovimiento = 0;
            int lError = 0;
            int idDocumentoUseless = idDocumento;
            if (!_transactionInProgress)
            {
                throw new SDKException("No se puede agregar un documento con movimiento sin una transacción activa.");
            }
            try
            {
                await Task.Run(() =>
                {
                    lError = SDK.fAltaMovimiento(idDocumentoUseless, ref idMovimiento, ref movimiento);
                    if (lError != 0)
                    {
                        throw new SDKException($"Error Dando de alta el movimiento: ", lError);
                    }
                });
                return idDocumento;
            }
            catch { throw; }
        }

        public async Task UpdateUnidadesMovimiento(int idMovimiento, string unidades)
        {
            await Task.Run(() =>
            {
                var lError = SDK.fBuscarIdMovimiento(idMovimiento);
                if (lError != 0)
                {
                    throw new SDKException("Error buscando el movimiento: ", lError);
                }

                lError = SDK.fEditarMovimiento();
                if (lError != 0)
                {
                    throw new SDKException("Error cambiando a estado de edicion de el movimiento: ", lError);
                }

                lError = SDK.fSetDatoMovimiento("CUNIDADES", unidades);
                if (lError != 0)
                {
                    throw new SDKException($"Error estableciendo las unidades ({unidades}) del movimiento: ", lError);
                }

                lError = SDK.fGuardaMovimiento();
                if (lError != 0)
                {
                    throw new SDKException("Error guardando el movimiento: ", lError);
                }
            });
        }

        public async Task SetDatosMovimientos(Dictionary<string, string> datosMovimientos, int idMovimiento)
        {
                await Task.Run(() =>
                {
                    var lError = SDK.fBuscarIdMovimiento(idMovimiento);
                    if (lError != 0)
                    {
                        throw new SDKException("Error buscando el movimiento: ", lError);
                    }

                    lError = SDK.fEditarMovimiento();
                    if (lError != 0)
                    {
                        throw new SDKException("Error cambiando a estado de edicion de el movimiento: ", lError);
                    }

                    foreach (var dato in datosMovimientos.Keys)
                    {
                        lError = SDK.fSetDatoMovimiento(dato, datosMovimientos[dato]);
                        if (lError != 0)
                        {
                            throw new SDKException($"Error estableciendo el dato: {dato} con valor: {datosMovimientos[dato]} en el movimiento: ", lError);
                        }
                    }

                    lError = SDK.fGuardaMovimiento();
                    if (lError != 0)
                    {
                        throw new SDKException("Error guardando el movimiento: ", lError);
                    }
                });
        }

        #endregion

        #region Producto Methods



        #endregion
    }
}
