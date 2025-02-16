using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Postgres
{
    public class DocumentoRepo : IDocumentoDtoRepo
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<DocumentoDto> _documentos;
        private readonly IMovimientoDtoRepo _movimientoRepo;

        public DocumentoRepo(PostgresCPEContext dbContext, IMovimientoDtoRepo movimientoRepo)
        {
            _movimientoRepo = movimientoRepo;
            _documentos = dbContext.documentos;
            _dbContext = dbContext;
        }

        public async Task<int> AddDocumentoAndMovimientoAsync(DocumentoDto documento, List<MovimientoDto> movimientos)
        {
            await _documentos.AddAsync(documento);

            await _dbContext.SaveChangesAsync();//save changes to get the id

            //now that we have the document id, add it to the movements
            movimientos.ForEach(m => m.IdDocumento = documento.IdPostgres);

            await _movimientoRepo.AddMovimientosAsync(movimientos);

            await _dbContext.SaveChangesAsync();

            return documento.IdPostgres;

        }

        public async Task<List<DocumentoDto>> GetDocumentosPendientes()
        {
            return await _documentos.AsNoTracking().Where(d => d.Impreso == false && d.IdContpaqiSQL == 0).ToListAsync();
        }

        public async Task UpdateDocumentoAsync(DocumentoDto documento)
        {
            if (documento.IdPostgres == 0)
            {
                throw new Exception("No se puede actualizar un documento sin id de interfaz");
            }
            if (documento.IdContpaqiSQL == 0)
            {
                throw new Exception("No se puede actualizar un documento sin id de contpaqi");
            }
            if (documento.Impreso == false)
            {
                throw new Exception("No se puede actualizar un documento sin marcar como impreso");
            }

            //update the document by its id
            var docToUpdate = await _documentos.FirstOrDefaultAsync(d => d.IdPostgres == documento.IdPostgres);
            if (docToUpdate == null)
            {
                throw new Exception("Documento no encontrado en la base de datos");
            }
            _dbContext.Entry(docToUpdate).CurrentValues.SetValues(documento);
            await _dbContext.SaveChangesAsync();
        }
    }
}
