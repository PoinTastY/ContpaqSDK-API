using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.UseCases.SDK
{
    public class SetDocumentoImpresoSDKUseCase
    {
        private readonly ISDKRepo _sdkRepo;
        private readonly ILogger _logger;
        public SetDocumentoImpresoSDKUseCase(ISDKRepo sdkRepo, ILogger logger)
        {
            _sdkRepo = sdkRepo;
            _logger = logger;
        }

        public async Task Execute(int idDocumento)
        {
            _logger.Log("Ejecutando caso de uso SetDocumentoImpresoSDKUseCase");
            var canWork = await _sdkRepo.StartTransaction();
            try
            {
                if (canWork)
                {
                    await _sdkRepo.SetImpreso(idDocumento, true);
                    _logger.Log($"Documento marcado como impreso. ID: {idDocumento}");
                    _sdkRepo.StopTransaction();
                }
                else
                {
                    _logger.Log("No se pudo iniciar la transacción para el caso de uso SetDocumentoImpreso.");
                    throw new SDKException("No se pudo iniciar la transacción para el caso de uso SetDocumentoImpreso.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Error en el caso de uso SetDocumentoImpreso.");
                throw new SDKException($"Error en el caso de uso SetDocumentoImpreso: {ex.Message}");
            }
            finally
            {
                _sdkRepo.StopTransaction();
            }
        }
    }
}
