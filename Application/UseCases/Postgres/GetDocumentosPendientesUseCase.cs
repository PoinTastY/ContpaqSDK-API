using Domain.Entities.Interfaces;
using Domain.Interfaces.Repos.PostgreRepo;
using Domain.Interfaces.Services;

namespace Application.UseCases.Postgres
{
    public class GetDocumentosPendientesUseCase
    {
        private readonly IDocumentoRepo _postgresRepository;
        private readonly ILogger _logger;

        public GetDocumentosPendientesUseCase(IDocumentoRepo postgresRepository, ILogger logger)
        {
            _postgresRepository = postgresRepository;
            _logger = logger;
        }

        public async Task<List<Documento>> Execute()
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
