using Core.Domain.Exceptions;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Core.Application.UseCases.SDK
{
    public class TestSDKUseCase
    {
        private readonly ISDKRepo _sdkRepo;
        private readonly ILogger _logger;
        public TestSDKUseCase(ISDKRepo sdkRepo, ILogger logger)
        {
            _sdkRepo = sdkRepo;
            _logger = logger;
        }

        /// <summary>
        /// Prueba para abrir y cerrar una transacción, sin hacer nada, solo para probar que el SDK funciona correctamente.
        /// </summary>
        /// <returns>NO excepcion si todo bien xd</returns>
        /// <exception cref="SDKException"></exception>
        public async Task Execute()
        {
            _logger.Log("Ejecutando caso de uso TestSDKUseCase...");

            while (true)
            {
                var canWork = await _sdkRepo.StartTransaction("test");
                if (canWork)
                {
                    _logger.Log("Transacción de prueba iniciada con éxito.");
                    _sdkRepo.StopTransaction();
                    _logger.Log("Transacción de prueba finalizada con éxito.");
                    return;
                }
                else
                {
                    _logger.Log("SDK Ocupado, esperando turno para TestSDKUseCase...");
                    await Task.Delay(1000);
                }
            }
        }
    }
}
