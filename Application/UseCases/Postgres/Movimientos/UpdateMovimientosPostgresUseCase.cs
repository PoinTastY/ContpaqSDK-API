using Domain.Entities.Interfaces;
using Domain.Interfaces.Services;

namespace Application.UseCases.Postgres.Movimientos
{
    public class UpdateMovimientosPostgresUseCase
    {
        private readonly Domain.Interfaces.Repos.PostgreRepo.IMovimientoRepo _movimientoRepo;
        private readonly ILogger _logger;

        public UpdateMovimientosPostgresUseCase(Domain.Interfaces.Repos.PostgreRepo.IMovimientoRepo movimientoRepo, ILogger logger)
        {
            _movimientoRepo = movimientoRepo;
            _logger = logger;
        }

        public async Task Execute(List<Movimiento> movimientos)
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
