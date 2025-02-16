using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;

namespace Infrastructure.Repositories.Postgres
{
    public class MovimientoRepo : IMovimientoDtoRepo
    {
        private readonly DbSet<MovimientoDto> _movimientos;
        private readonly PostgresCPEContext _dbContext;

        public MovimientoRepo(PostgresCPEContext dbContext)
        {
            _movimientos = dbContext.movimientos;
            _dbContext = dbContext;
        }

        public async Task AddMovimientosAsync(List<MovimientoDto> movimientos)
        {
            await _dbContext.AddRangeAsync(movimientos);
        }

        public async Task<List<MovimientoDto>> GetMovimientosByDocumentoIdAsync(int id)
        {
            return await _movimientos.Where(m => m.IdDocumento == id).ToListAsync();
        }

        public async Task UpdateMovimientos(List<MovimientoDto> movimientos)
        {
            _dbContext.UpdateRange(movimientos);
            await _dbContext.SaveChangesAsync();
        }
    }
}
