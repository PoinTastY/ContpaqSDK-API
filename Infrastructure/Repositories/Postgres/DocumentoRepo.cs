using Domain.Entities.Interfaces;
using Domain.Interfaces.Repos.PostgreRepo;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Postgres
{
    public class DocumentoRepo : IDocumentoRepo
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Documento> _documentos;
        private readonly IMovimientoRepo _movimientoRepo;

        public DocumentoRepo(PostgresCPEContext dbContext, IMovimientoRepo movimientoRepo)
        {
            _movimientoRepo = movimientoRepo;
            _documentos = dbContext.Set<Documento>();
            _dbContext = dbContext;
        }

        public async Task<int> AddDocumentoAndMovimientoAsync(Documento documento, List<Movimiento> movimientos)
        { 
            await _documentos.AddAsync(documento);
            await _movimientoRepo.AddMovimientosAsync(movimientos);
            await _dbContext.SaveChangesAsync();
            return documento.IdInterfaz;

        }

        public async Task<List<Documento>> GetDocumentosPendientes()
        {
            return await _documentos.Where(d => d.Impreso == false).ToListAsync();
        }

    }
}
