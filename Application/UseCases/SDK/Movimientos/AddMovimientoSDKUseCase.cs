using Application.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.UseCases.SDK.Movimientos
{
    public class AddMovimientoSDKUseCase
    {
        private readonly ISDKRepo _sdkRepo;
        private readonly ILogger _logger;
        public AddMovimientoSDKUseCase(ISDKRepo sdkRepo, ILogger logger)
        {
            _sdkRepo = sdkRepo;
            _logger = logger;
        }

        public async Task<MovimientoDTO> Execute(MovimientoDTO movimiento)
        {
            _logger.Log("Ejecutando caso de uso AddMovimientoSDKUseCase");

            var canWork = await _sdkRepo.StartTransaction();
            try
            {
                if (canWork)
                {
                    var movimientoStruct = movimiento.GetSDKMovementStruct();

                    movimiento.CIDMOVIMIENTO = await _sdkRepo.AddMovimiento(movimientoStruct, movimiento.CIDDOCUMENTO);

                    _logger.Log($"Movimiento agregado con éxito. ID: {movimiento.CIDMOVIMIENTO}");

                    _sdkRepo.StopTransaction();

                    return movimiento;
                }
                else
                {
                    _logger.Log("No se pudo iniciar la transacción para el caso de uso AddMovimientoSDKUseCase.");
                    throw new SDKException("No se pudo iniciar la transacción para el caso de uso AddMovimientoSDKUseCase.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Error en el caso de uso AddMovimientoSDKUseCase.");
                throw new SDKException($"Error en el caso de uso AddMovimientoSDKUseCase: {ex.Message}");
            }
            finally
            {
                _sdkRepo.StopTransaction();
            }
        }
    }
}
