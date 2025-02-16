using Core.Domain.Entities.SQL;
using Core.Domain.Interfaces.Repositories.SQL;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DocumentRepo : IDocumentoSQLRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly DbSet<DocumentoSQL> _documents;
        private readonly DbSet<ConceptoSQL> _concepts;


        public DocumentRepo(ContpaqiSQLContext context)
        {
            _context = context;
            _documents = _context.Set<DocumentoSQL>();
            _concepts = _context.Set<ConceptoSQL>();
        }

        public async Task<IEnumerable<DocumentoSQL>> GetRangeByFechaConceptoSerieAsync(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto, string serie, CancellationToken cancellationToken = default)
        {
            
            var concepto = await _concepts.AsNoTracking().Where(c => c.CCODIGOCONCEPTO == codigoConcepto).FirstOrDefaultAsync();
            if (concepto == null)
            {
                throw new KeyNotFoundException($"Parece que el concepto con codigo: {codigoConcepto}, no existe :c");
            }

            return await _documents.AsNoTracking().Where(
                d =>
                d.CFECHA >= fechaInicio && d.CFECHA <= fechaFin &&
                d.CIDCONCEPTODOCUMENTO == concepto.CIDCONCEPTODOCUMENTO && d.CSERIEDOCUMENTO == serie )
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<DocumentoSQL>> GetRangeByFechaSerieAsync(DateTime fechaInicio, DateTime fechaFin, string serie, CancellationToken cancellationToken = default)
        {
            
            var pedidos =  await _documents.AsNoTracking().Where(d =>
                d.CFECHA >= fechaInicio && d.CFECHA <= fechaFin &&
                d.CSERIEDOCUMENTO == serie)
                .ToListAsync(cancellationToken);

            if (pedidos.Count() == 0)
                throw new KeyNotFoundException($"No se encontraron documentos para la serie: {serie}");
            return pedidos;
        }

        public async Task<DocumentoSQL> GetByFolioAndSerieAsync(string folio, string serie, CancellationToken cancellationToken = default)
        {
            double folioDouble = double.Parse(folio);
            var result = await _documents.AsNoTracking().Where(
                d => d.CFOLIO == folioDouble &&
                d.CSERIEDOCUMENTO == serie)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                throw new NotFoundArgumentException($"No se encontraron coincidencias para el folio: {folio}");

            return result;
        }

        public async Task<DocumentoSQL> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _documents.AsNoTracking().Where(
                d => d.CIDDOCUMENTO == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                throw new NotFoundArgumentException($"No se encontraron coincidencias para el id: {id}");
            
            return result;
        }


        public async Task<IEnumerable<DocumentoSQL>> GetByIdClienteAndDateAsync(int idCliente, DateTime fechaInicio, DateTime fechaFin, CancellationToken cancellationToken = default)
        {
            var documentos = await _documents.AsNoTracking().Where(
                d => d.CIDCLIENTEPROVEEDOR == idCliente &&
                d.CFECHA >= fechaInicio &&
                d.CFECHA <= fechaFin)
                .ToListAsync(cancellationToken);

            if (documentos.Count() == 0)
                throw new KeyNotFoundException($"No se encontraron documentos para el cliente: {idCliente} entre las fechas: {fechaInicio} y {fechaFin}");

            return documentos;
        }
    }
}
