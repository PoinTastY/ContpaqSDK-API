using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.Postgres.Movimientos
{
    public class UpdateMovimientosPostgresUseCase
    {
        private readonly IMovimientoDtoRepo _movimientoRepo;
        private readonly ILogger _logger;

        public UpdateMovimientosPostgresUseCase(IMovimientoDtoRepo movimientoRepo, ILogger logger)
        {
            _movimientoRepo = movimientoRepo;
            _logger = logger;
        }

        public async Task Execute(List<MovimientoDto> movimientos)
        {
            _logger.Log("Ejecutando caso de uso UpdateMovimientosUseCase...");
            foreach (var movimiento in movimientos)
            {
                await _movimientoRepo.UpdateMovimientos(movimientos);
            }
            _logger.Log($"Unidades de movimientos actualizados con éxito.");
        }
    }
}
