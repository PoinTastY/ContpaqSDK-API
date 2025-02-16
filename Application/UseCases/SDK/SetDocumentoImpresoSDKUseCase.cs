using Core.Domain.Exceptions;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Core.Application.UseCases.SDK
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

        public async Task Execute(int idDocumento, string empresa)
        {
            _logger.Log("Ejecutando caso de uso SetDocumentoImpresoSDK...");
            while (true)
            {
                var canWork = await _sdkRepo.StartTransaction(empresa);
                if (canWork)
                {
                    await _sdkRepo.SetImpreso(idDocumento, true);
                    _sdkRepo.StopTransaction();

                    _logger.Log($"Documento {idDocumento}, establecido como impreso");
                    return;
                }
                else
                {
                    _logger.Log("SDK Ocupado, esperando turno para SetDocumentoImpresoSDK...");
                    await Task.Delay(1000);
                }
            }
        }
    }
}
