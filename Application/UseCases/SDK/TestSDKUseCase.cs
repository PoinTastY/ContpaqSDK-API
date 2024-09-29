using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.UseCases.SDK
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
            _logger.Log("Ejecutando caso de uso TestSDKUseCase");
            var canWork = await _sdkRepo.StartTransaction();
            try
            {
                if (canWork)
                {
                    _logger.Log("Transacción iniciada con éxito.");
                    _sdkRepo.StopTransaction();
                    _logger.Log("Transacción finalizada con éxito.");
                    return;
                }
                else
                {
                    _logger.Log("No se pudo iniciar la transacción para el caso de uso TestSDKUseCase.");
                    throw new SDKException("No se pudo iniciar la transacción para el caso de uso TestSDKUseCase.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Error en el caso de uso TestSDKUseCase.");
                throw new SDKException($"Error en el caso de uso TestSDKUseCase: {ex.Message}");
            }
            finally
            {
                _sdkRepo.StopTransaction();
            }
        }
    }
}
