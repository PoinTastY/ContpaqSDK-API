using Domain.Entities.Interfaces;
using Domain.Interfaces.Repos.PostgreRepo;
using Domain.Interfaces.Services;

namespace Application.UseCases.Postgres
{
    public class UpdateDocumentoPendientePostgresUseCase
    {
        private readonly IDocumentoRepo _postgresRepository;
        private readonly ILogger _logger;
        public UpdateDocumentoPendientePostgresUseCase(IDocumentoRepo postgresRepository, ILogger logger)
        {
            _postgresRepository = postgresRepository;
            _logger = logger;
        }

        public async Task Execute(Documento documento)
        {
            _logger.Log($"Actualizando documento pendiente en Postgres Id: {documento.IdInterfaz}, Id Contpaqi: {documento.IdContpaqiSQL}");
            try
            {
                await _postgresRepository.UpdateDocumentoAsync(documento);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error al actualizar documento pendiente en Postgres: {ex.Message} (Inner: {ex.InnerException})");
                throw new Exception($"Error al actualizar documento pendiente en Postgres: {ex.Message} (Inner: {ex.InnerException})");
            }

        }
    }
}
