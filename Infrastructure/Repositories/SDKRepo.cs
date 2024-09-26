using Domain.Exceptions;
using Domain.Entities.Estructuras;
using Domain.Interfaces;
using Domain.SDK_Comercial;
using System.Data;
using System.Text;
using Document = Domain.Entities.Document;

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

        public SDKRepo(SDKSettings settings)
        {
            _nombrePAQ = settings.NombrePAQ;
            _dirEmpresa = settings.RutaEmpresa;
            _user = settings.User;
            _password = settings.Password;
            _dirBinarios = settings.RutaBinarios;
            _settings = settings;

            Initialize();
        }

        public SDKSettings GetSDKSettings()
        {
            return _settings;
        }

        public string GetBinariesDir()
        {
            return _dirBinarios;
        }

        public void Initialize()
        {
            try
            {
                SDK.SetCurrentDirectory(_dirBinarios);
                SDK.SetDllDirectory(_dirBinarios);


                var attempts = 0;
                int lError;

                // Verifica si la DLL existe en el directorio actual
                string dllPath = Path.Combine(_dirBinarios, "MGWServicios.dll");
                if (!File.Exists(dllPath))
                {
                    throw new SDKException("No se encontro MGWServicios.dll en el directorio especificado.");
                }

                try
                {
                    SDK.fInicioSesionSDK(_user, _password);
                }
                catch (Exception ex)
                {
                    throw new SDKException("No se pudo iniciar sesion en el SDK: " + ex);
                }

                //indicar con que sistema se va a trabajar
                while (true)
                {
                    try
                    {
                        var directory = Directory.GetCurrentDirectory();
                        lError = SDK.fSetNombrePAQ(_nombrePAQ);
                        if (lError != 0)
                        {
                            var error = SDK.rError(lError);
                            System.Threading.Thread.Sleep(10000);
                            if (attempts > 5)
                            {
                                throw new SDKException($"Despues de {attempts} intentos, no se pudo establecer el nombrePAQ: ", lError);
                            }

                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new SDKException("Error al establecer el nombrePAQ: " + ex);
                    }
                }

                attempts = 0;
                while (true)
                {
                    lError = SDK.fAbreEmpresa(_dirEmpresa);
                    if (lError != 0)
                    {
                        if (++attempts > 4)
                        {
                            throw new SDKException($"No se pudo abrir la empresa: {_dirEmpresa}, ", lError);
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
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

        private async void StartTransaction()
        {
            if (_transactionInProgress)
                return;
            int attempts = 0;
            int lError;
            try
            {
                await Task.Run(() =>
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
                            return;
                        }
                    }
                });
            }
            catch { throw; }
        }

        private void FinishTransaction()
        {
            if (_transactionInProgress)
            {
                _transactionInProgress = false;
                SDK.fCierraEmpresa();
            }
        }

        public async Task<int> AddDocumentWithMovement(tDocumento documento, tMovimiento movimiento)
        {
            int idDocumento = 0;
            StartTransaction();
            try
            {
                idDocumento = await AddDocument(documento);
                try
                {
                    await AddMovimiento(movimiento, idDocumento);

                }
                catch(Exception ex)
                {
                    throw new Exception($"Se agrego el documento, pero hubo un problema creando el movimiento: {ex.Message}");
                }

                return idDocumento;
            }
            catch { throw; }
            finally { FinishTransaction(); }
        }

        public async Task SetDatoDocumento(Dictionary<string, string> camposValores, int idDocumento)
        {
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
            catch { }
            finally { FinishTransaction(); }
        }

        public async Task<int> AddDocument(tDocumento documento)
        {
            int lError = 0;
            int idDocumento = 0;
            StartTransaction();
            try
            {
                await Task.Run(() =>
                {
                    //validar CteProv
                    lError = SDK.fBuscaCteProv(documento.aCodigoCteProv);
                    if (lError != 0)
                    {
                        throw new SDKException($"Error Agregando el documento: No se encontro el codigo de Cliente Proveedor: ", lError);
                    }

                    double folio = 0;
                    lError = SDK.fSiguienteFolio(documento.aCodConcepto, new StringBuilder(documento.aSerie), ref folio);
                    if (lError != 0)
                    {
                        throw new SDKException($"Problema obteniendo el siguiente folio. Concepto: {documento.aCodConcepto}, Serie: {documento.aSerie}: ");
                    }

                    lError = SDK.fAltaDocumento(ref idDocumento, ref documento);
                    if (lError != 0)
                    {
                        throw new SDKException($"Error dando de alta el documento: ", lError);
                    }
                });

                return idDocumento;
            }
            catch { throw; }
            finally { FinishTransaction(); }
        }

        public async Task<int> AddMovimiento(tMovimiento movimiento, int idDocumento)
        {
            int idMovimiento = 0;
            int lError = 0;
            StartTransaction();
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
            finally { FinishTransaction(); }
        }


        public async Task SetImpreso(int idDocumento, bool impressed)
        {
            StartTransaction();
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
            finally { FinishTransaction(); }
        }
    }
}
