using Domain.Entities;
using Domain.Interfaces.Repos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovimientoRepo : IMovimientoRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly DbSet<MovimientoSQL> _movimientos;
        public MovimientoRepo(ContpaqiSQLContext context)
        {
            _context = context;
            _movimientos = _context.Set<MovimientoSQL>();
        }

        public async Task<List<MovimientoSQL>> GetMovimientosByDocumentId(int idDocumento)
        {
            return await _movimientos.AsNoTracking().Where(m => m.CIDDOCUMENTO == idDocumento).ToListAsync();
        }

        public async Task<List<int>> GetMovimientosIdsByDocumenId(int idDocumento)
        {
            return await _movimientos.AsNoTracking().Where(m => m.CIDDOCUMENTO == idDocumento).Select(m => m.CIDMOVIMIENTO).ToListAsync();
        }
    }
}
