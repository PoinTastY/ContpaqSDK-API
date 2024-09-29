using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repos;
using Domain.SDK_Comercial;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DocumentRepo : IDocumentRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly DbSet<DocumentSQL> _documents;
        private readonly DbSet<ConceptoSQL> _concepts;


        public DocumentRepo(ContpaqiSQLContext context)
        {
            _context = context;
            _documents = _context.Set<DocumentSQL>();
            _concepts = _context.Set<ConceptoSQL>();
        }


        public async Task<List<DocumentSQL>> GetAllDocumentsByFechaConceptoSerieAsync(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto, string serie)
        {
            try
            {
                var concepto = await _concepts.AsNoTracking().Where(c => c.CCODIGOCONCEPTO == codigoConcepto).FirstOrDefaultAsync();
                if (concepto == null)
                {
                    throw new NotFoundArgumentException($"Parece que el concepto con codigo: {codigoConcepto}, no existe :c");
                }

                return await _documents.AsNoTracking().Where(d =>
                d.CFECHA >= fechaInicio && d.CFECHA <= fechaFin &&
                d.CIDCONCEPTODOCUMENTO == concepto.CIDCONCEPTODOCUMENTO && d.CSERIEDOCUMENTO == serie ).ToListAsync();
            }
            catch (NotFoundArgumentException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrio un error inesperado al conseguir los documentos; " + e.Message);
            }
        }

        public async Task<List<DocumentSQL>> GetAllDocumentsByFechaSerieCPEAsync(DateTime fechaInicio, DateTime fechaFin, string serie)
        {
            try 
            { 
                return await _documents.AsNoTracking().Where(d =>
                d.CFECHA >= fechaInicio && d.CFECHA <= fechaFin &&
                d.CSERIEDOCUMENTO == serie && d.CTEXTOEXTRA3 == string.Empty).ToListAsync();
            }
            catch (NotFoundArgumentException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("Ocurrio un error inesperado al conseguir los documentos; " + e.Message);
            }
        }


        public async Task<List<DocumentSQL>> GetAllDocumentsByFechaAndSerieAsync(DateTime fechaInicio, DateTime fechaFin, string serie)
        {
            try
            {
                return await _documents.AsNoTracking().Where(d => d.CFECHA >= fechaInicio && d.CFECHA <= fechaFin &&
                d.CSERIEDOCUMENTO.Contains(serie)).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DocumentSQL> GetDocumentByFolioAndSerieAsync(string folio, string serie)
        {
            try
            {
                double folioDouble = double.Parse(folio);
                var result = await _documents.AsNoTracking().Where(d => d.CFOLIO == folioDouble &&
                d.CSERIEDOCUMENTO == serie).FirstOrDefaultAsync();

                if (result == null)
                    throw new NotFoundArgumentException($"No se encontraron coincidencias para el folio: {folio}");

                return result;
            }
            catch (NotFoundArgumentException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrio un error inesperado al buscar los documentos para el folio: {folio}, " + e.Message);
            }
        }



        public async Task<bool> IdExistAsync(int id)
        {
            return await _documents.AsNoTracking().AnyAsync(d => d.CIDDOCUMENTO == id);
        }
    }
}
