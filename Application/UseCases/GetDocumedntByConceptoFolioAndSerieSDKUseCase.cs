using Application.DTOs;
using Domain.Entities.Estructuras;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases
{
    public class GetDocumedntByConceptoFolioAndSerieSDKUseCase
    {
        private readonly ISDKRepo _sDKRepo;
        private readonly ILogger _logger;

        public GetDocumedntByConceptoFolioAndSerieSDKUseCase(ISDKRepo sDKRepo, ILogger logger)
        {
            _logger = logger;
            _sDKRepo = sDKRepo;
        } 

        public async Task<DocumentDTO> Execute(string codConcepto,  string serie, string folio)
        {
            var canWork = await _sDKRepo.StartTransaction();
            if (canWork)
            {
                _logger.Log("Ejecutando caso de uso GetDocumedntByFolioAndSerieSDKUseCase");
                try
                {
                    var dictionary = await _sDKRepo.GetDocumentoByConceptoFolioAndSerie(codConcepto, serie, folio);
                    tDocumento documento = dictionary.First().Value;
                    _logger.Log($"Documento encontrado. Folio: {documento.aFolio}, Concepto: {documento.aCodConcepto}, Serie: {documento.aSerie}");

                    var dto = new DocumentDTO(documento, dictionary.First().Key);
                    return dto;
                }
                catch (Exception ex)
                {
                    _logger.Log($"Error en el caso de uso GetDocumentByConceptoFolioAndSerieSDKUseCase: {ex.Message}.");
                    throw new SDKException($"Error en el caso de uso GetDocumentByConceptoFolioAndSerieSDK: {ex.Message}");
                }
                finally
                {
                    _sDKRepo.StopTransaction();
                }
            }
            else
            {
                _logger.Log("No se pudo iniciar la transacción para el caso de uso GetDocumentByConceptoFolioAndSerieSDK.");
                throw new SDKException("No se pudo iniciar la transacción para el caso de uso GetDocumentByConceptoFolioAndSerieSDK.");
            }
        }
    }
}
