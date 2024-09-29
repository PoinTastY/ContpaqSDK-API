using Application.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.UseCases.SDK.Documentos
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

        /// <summary>
        /// Ask for a document by concept, serie and folio
        /// </summary>
        /// <param name="codConcepto"></param>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <returns>DocumentDTO instance</returns>
        /// <exception cref="SDKException"></exception>
        public async Task<DocumentDTO> Execute(string codConcepto, string serie, string folio)
        {
            var canWork = await _sDKRepo.StartTransaction();
            if (canWork)
            {
                _logger.Log("Ejecutando caso de uso GetDocumedntByFolioAndSerieSDKUseCase");
                try
                {
                    var documento = await _sDKRepo.GetDocumentoByConceptoFolioAndSerie(codConcepto, serie, folio);

                    _logger.Log($"Documento encontrado. Folio: {documento.CFOLIO}, Concepto: {codConcepto}, Serie: {documento.CSERIEDOCUMENTO}");

                    var dto = new DocumentDTO(documento);
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
