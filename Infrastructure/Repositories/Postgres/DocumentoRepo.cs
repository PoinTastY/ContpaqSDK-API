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

        public async Task<DocumentoDto> AddAsync(DocumentoDto documento, CancellationToken cancellationToken)
        {
            await _documentos.AddAsync(documento);

            await _dbContext.SaveChangesAsync(cancellationToken);//save changes to get the id

            if (documento.IdPostgres == 0)
                throw new Exception("No se pudo agregar el documento a la base de datos");

            return documento;
        }

        public async Task<IEnumerable<DocumentoDto>> GetPendientesAsync(CancellationToken cancellationToken)
        {
            var documentos = await _documentos.AsNoTracking().Where(
                d => d.Impreso == false && d.IdContpaqiSQL == 0)
                .ToListAsync(cancellationToken);

            if (documentos.Count == 0)
                throw new KeyNotFoundException("No se encontraron documentos pendientes");

            return documentos;
        }

        public async Task<DocumentoDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var documento = await _documentos.AsNoTracking().FirstOrDefaultAsync(
                d => d.IdPostgres == id,
                cancellationToken);

            if (documento == null)
                throw new KeyNotFoundException($"No se encontro el documento con id: {id}");

            return documento;
        }

        public async Task UpdateAsync(DocumentoDto documento, CancellationToken cancellationToken)
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
            var docToUpdate = await _documentos.FirstOrDefaultAsync(d => d.IdPostgres == documento.IdPostgres, cancellationToken);
            if (docToUpdate == null)
            {
                throw new Exception("Documento no encontrado en la base de datos");
            }
            _dbContext.Entry(docToUpdate).CurrentValues.SetValues(documento);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
        {
            var documento = await _documentos.FirstOrDefaultAsync(d => d.IdPostgres == id, cancellationToken);
            if (documento == null)
            {
                throw new KeyNotFoundException($"No se encontro el documento con id: {id}");
            }
            _documentos.Remove(documento);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
