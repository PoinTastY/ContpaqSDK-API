using Application.DTOs;
using Domain.Entities.Estructuras;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.UseCases.SDK.Documentos
{
    public class AddDocumentAndMovementsSDKUseCase
    {
        private readonly ISDKRepo _sdkRepo;
        private readonly ILogger _logger;

        public AddDocumentAndMovementsSDKUseCase(ISDKRepo sDKRepo, ILogger logger)
        {
            _logger = logger;
            _sdkRepo = sDKRepo;
        }

        public async Task<DocumentDTO> Execute(DocumentoConMovimientosDTO request)
        {
            _logger.Log("Ejecutando caso de uso AddDocumentAndMovements...");

            var canWork = await _sdkRepo.StartTransaction();
            try
            {
                if (canWork)
                {
                    var documento = new tDocumento()
                    {
                        aCodConcepto = request.Documento.aCodConcepto,
                        aSerie = request.Documento.aSerie,
                        aFecha = request.Documento.aFecha,
                        aCodigoCteProv = request.Documento.aCodigoCteProv,
                        aReferencia = request.Documento.aReferencia
                    };

                    var listaMovimientos = request.Movimientos.Select(mov => mov.GetSDKMovementStruct()).ToList();

                    var documentoDTO = await _sdkRepo.AddDocumentAndMovements(documento, listaMovimientos);

                    _logger.Log($"Documento agregado con éxito. ID: {documentoDTO.CIDDOCUMENTO}");

                    if(request.Documento.CIMPRESO == 1)
                    {
                        await _sdkRepo.SetImpreso(documentoDTO.CIDDOCUMENTO, true);
                        _logger.Log($"Documento marcado como impreso. ID: {documentoDTO.CIDDOCUMENTO}");
                    }

                    return new DocumentDTO(documentoDTO);
                }
                else
                {
                    _logger.Log("No se pudo iniciar la transacción para el caso de uso AddDocumentAndMovements.");
                    throw new SDKException("No se pudo iniciar la transacción para el caso de uso AddDocumentAndMovements.");
                }

                //if documento.CIMPRESO == 1 set the document as printed
            }
            catch (Exception ex)
            {
                _logger.Log("Error en el caso de uso AddDocumentAndMovements.");
                throw new SDKException($"Error en el caso de uso AddDocumentAndMovements: {ex.Message}");
            }
            finally
            {
                _sdkRepo.StopTransaction();
            }
        }
    }
}
