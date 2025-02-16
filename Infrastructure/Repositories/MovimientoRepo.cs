using Core.Domain.Entities.SQL;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovimientoRepo : IMovimientoSQLRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly DbSet<MovimientoSQL> _movimientos;

        public MovimientoRepo(ContpaqiSQLContext context)
        {
            _context = context;
            _movimientos = _context.Set<MovimientoSQL>();
        }

        public async Task<IEnumerable<MovimientoSQL>> GetByDocumentoId(int idDocumento, CancellationToken cancellationToken)
        {
            var movimientos = await _movimientos.AsNoTracking().Where(
                m => m.CIDDOCUMENTO == idDocumento)
                .ToListAsync(cancellationToken);

            if (movimientos.Count() == 0)
                throw new KeyNotFoundException($"No se encontraron movimientos para el documento con id: {idDocumento}");

            return movimientos;
        }

        public async Task<IEnumerable<int>> GetMovimientosIdsByDocumentId(int idDocumento, CancellationToken cancellationToken)
        {
            var movimientos = await _movimientos.AsNoTracking().Where(
                m => m.CIDDOCUMENTO == idDocumento).ToListAsync(cancellationToken);

            if (movimientos.Count() == 0)
                throw new KeyNotFoundException($"No se encontraron movimientos para el documento con id: {idDocumento}");

            return movimientos.Select(m => m.CIDMOVIMIENTO).ToList();
        }
    }
}
