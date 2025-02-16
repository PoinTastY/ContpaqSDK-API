using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;
using Core.Domain.Interfaces.Services;

namespace Core.Application.UseCases.Postgres
{
    public class UpdateDocumentoPendienteDtoUseCase
    {
        private readonly IDocumentoDtoRepo _documentoDtoRepo;
        private readonly ILogger _logger;
        public UpdateDocumentoPendienteDtoUseCase(IDocumentoDtoRepo documentoDtoRepo, ILogger logger)
        {
            _documentoDtoRepo = documentoDtoRepo;
            _logger = logger;
        }

        public async Task Execute(DocumentoDto documento)
        {
            _logger.Log($"Actualizando documento pendiente en Postgres Id: {documento.IdPostgres}, Id Contpaqi: {documento.IdContpaqiSQL}");

            await _documentoDtoRepo.UpdateAsync(documento);

            _logger.Log($"Documento pendiente actualizado en Postgres Id: {documento.IdPostgres}, Id Contpaqi: {documento.IdContpaqiSQL}");

            return;
        }
    }
}
