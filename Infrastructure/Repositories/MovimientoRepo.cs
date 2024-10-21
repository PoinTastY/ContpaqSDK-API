using Domain.Entities;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovimientoRepo : IMovimientoRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly DbSet<MovimientoSQL> _movimientos;
        private readonly ILogger _logger;

        public MovimientoRepo(ContpaqiSQLContext context, ILogger logger)
        {
            _context = context;
            _movimientos = _context.Set<MovimientoSQL>();
            _logger = logger;
        }

        public async Task<List<MovimientoSQL>> GetMovimientosByDocumentId(int idDocumento)
        {
            return await _movimientos.AsNoTracking().Where(m => m.CIDDOCUMENTO == idDocumento).ToListAsync();
        }

        public async Task<List<int>> GetMovimientosIdsByDocumenId(int idDocumento)
        {
            _logger.Log("Ejecutando consulta de movimientos...");
            return await _movimientos.AsNoTracking().Where(m => m.CIDDOCUMENTO == idDocumento).Select(m => m.CIDMOVIMIENTO).ToListAsync();

        }

        public async Task UpdateUnidadesMovimientoById(int idMovimiento, double unidades)
        {
            var movimiento = await _movimientos.FirstOrDefaultAsync(m => m.CIDMOVIMIENTO == idMovimiento);
            if (movimiento != null)
            {
                movimiento.CUNIDADES = unidades;
                movimiento.CUNIDADESCAPTURADAS = unidades;
                movimiento.CTOTAL = movimiento.CPRECIO * unidades;
                movimiento.CNETO = movimiento.CTOTAL;
                movimiento.CUNIDADESPENDIENTES = unidades;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("No se encontró el movimiento con el id proporcionado");
            }
        }
    }
}
