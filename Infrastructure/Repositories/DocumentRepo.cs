using Domain.Entities;
using Domain.Interfaces;
using Domain.SDK_Comercial;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DocumentRepo : IDocumentRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly SDKSettings _sDKSettings;

        public DocumentRepo(ContpaqiSQLContext context, SDKSettings sDKSettings)
        {
            _context = context;
            _sDKSettings = sDKSettings;
        }

        /// <summary>
        /// Returns a list of documents by fecha, concepto and serie
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="codigoConcepto"></param>
        /// <param name="serie"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Document>> GetAllDocumentsByFechaConceptoSerieAsync(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto, string serie)
        {
            try
            {
                var concepto = _context.concepts.AsNoTracking().Where(c => c.CCODIGOCONCEPTO == codigoConcepto).Select(c => c.CIDCONCEPTODOCUMENTO)
                .FirstOrDefault();
                if (concepto == 0)
                {
                    throw new NotFoundArgumentException($"Parece que el concepto con codigo: {codigoConcepto}, no existe :c");
                }

                return await _context.documents.AsNoTracking().Where(d =>
                d.CFECHA >= fechaInicio && d.CFECHA <= fechaFin &&
                d.CIDCONCEPTODOCUMENTO == concepto && d.CSERIEDOCUMENTO == serie &&
                d.CMETODOPAG == _sDKSettings.CMETODOPAGO).ToListAsync();
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

        /// <summary>
        /// Needs a date range and a serie to return a list of documents
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="serie"></param>
        /// <returns>List of documents matching the serie</returns>
        /// <exception cref="NotFoundArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<List<Document>> GetAllDocumentsBySerieAsync(DateTime fechaInicio, DateTime fechaFin, string serie)
        {
            try
            {
                return await _context.documents.AsNoTracking().Where(d => d.CFECHA >= fechaInicio && d.CFECHA <= fechaFin &&
                d.CSERIEDOCUMENTO.Contains(serie) &&
                d.CMETODOPAG == _sDKSettings.CMETODOPAGO &&
                d.CIMPRESO == 0).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Search documents by folio
        /// </summary>
        /// <param name="folio"></param>
        /// <returns>Matching Folium</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Document> GetDocumentByFolioAsync(string folio, string serie)
        {
            try
            {
                double folioDouble = double.Parse(folio);
                var result = await _context.documents.AsNoTracking().Where(d => d.CFOLIO == folioDouble &&
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
    }
}
