using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;
using Core.Domain.Interfaces.Services;

namespace Core.Application.UseCases.Postgres
{
    public class GetDocumentosPendientesDtoUseCase
    {
        private readonly IDocumentoDtoRepo _documentoDtoRepo;
        private readonly ILogger _logger;

        public GetDocumentosPendientesDtoUseCase(IDocumentoDtoRepo documentoDtoRepo, ILogger logger)
        {
            _documentoDtoRepo = documentoDtoRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<DocumentoDto>> Execute()
        {
            _logger.Log("Obteniendo documentos pendientes de Postgres");

            var documentos = await _documentoDtoRepo.GetPendientesAsync();

            _logger.Log($"Se obtuvieron {documentos.Count()} documentos pendientes");

            return documentos;
        }
    }
}
