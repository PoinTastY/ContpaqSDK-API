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

        public async Task AddRangeAsync(IEnumerable<MovimientoDto> movimientos, CancellationToken cancellationToken)
        {
            await _movimientos.AddRangeAsync(movimientos, cancellationToken);
        }
        public async Task<MovimientoDto> AddAsync(MovimientoDto movimiento, CancellationToken cancellationToken)
        {
            await _movimientos.AddAsync(movimiento, cancellationToken);

            return movimiento;
        }

        public async Task<IEnumerable<MovimientoDto>> GetByDocumentoDtoIdAsync(int id, CancellationToken cancellationToken)
        {
            var movimientos = await _movimientos.AsNoTracking().Where(
                m => m.IdDocumento == id)
                .ToListAsync(cancellationToken);

            if (movimientos.Count() == 0)
                throw new KeyNotFoundException($"No se encontraron movimientos para el documento con id: {id}");

            return movimientos;
        }

        public async Task UpdateRangeAsync(IEnumerable<MovimientoDto> movimientos, CancellationToken cancellationToken)
        {
            _movimientos.UpdateRange(movimientos);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(MovimientoDto movimiento, CancellationToken cancellationToken)
        {
            _movimientos.Update(movimiento);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
