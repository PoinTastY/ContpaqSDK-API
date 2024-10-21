using Domain.Interfaces.Repos.PostgreRepo;
using Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Postgres
{
    public class MovimientoRepo : IMovimientoRepo
    {
        private readonly DbSet<Movimiento> _movimientos;
        private readonly PostgresCPEContext _dbContext;

        public MovimientoRepo(PostgresCPEContext dbContext)
        {
            _movimientos = dbContext.movimientos;
            _dbContext = dbContext;
        }

        public async Task AddMovimientosAsync(List<Domain.Entities.Interfaces.Movimiento> movimientos)
        {
            await _dbContext.AddRangeAsync(movimientos);
        }

        public async Task<List<Domain.Entities.Interfaces.Movimiento>> GetMovimientosByDocumentoIdAsync(int id)
        {
            return await _movimientos.Where(m => m.IdPedido == id).ToListAsync();
        }

        public async Task UpdateMovimientos(List<Domain.Entities.Interfaces.Movimiento> movimientos)
        {
            _dbContext.UpdateRange(movimientos);
            await _dbContext.SaveChangesAsync();
        }
    }
}
