using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.Postgres
{
    public class GetDocumentosPendientesUseCase
    {
        private readonly IDocumentoDtoRepo _postgresRepository;
        private readonly ILogger _logger;

        public GetDocumentosPendientesUseCase(IDocumentoDtoRepo postgresRepository, ILogger logger)
        {
            _postgresRepository = postgresRepository;
            _logger = logger;
        }

        public async Task<List<DocumentoDto>> Execute()
        {
            try
            {
                _logger.Log("Obteniendo documentos pendientes de Postgres");
                return await _postgresRepository.GetDocumentosPendientes();
            }
            catch (Exception ex)
            {
                _logger.Log($"Error al obtener documentos pendientes de Postgres: {ex.Message} (Inner: {ex.InnerException})");
                throw new Exception($"Error al obtener documentos pendientes de Postgres: {ex.Message} (Inner: {ex.InnerException})");
            }
        }
    }
}
