using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.Postgres
{
    public class UpdateDocumentoPendientePostgresUseCase
    {
        private readonly IDocumentoDtoRepo _postgresRepository;
        private readonly ILogger _logger;
        public UpdateDocumentoPendientePostgresUseCase(IDocumentoDtoRepo postgresRepository, ILogger logger)
        {
            _postgresRepository = postgresRepository;
            _logger = logger;
        }

        public async Task Execute(DocumentoDto documento)
        {
            _logger.Log($"Actualizando documento pendiente en Postgres Id: {documento.IdPostgres}, Id Contpaqi: {documento.IdContpaqiSQL}");
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
