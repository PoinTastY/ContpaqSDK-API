using Application.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.UseCases.SDK.Documentos
{
    public class AddDocumentWithMovementSDKUseCase
    {
        private readonly ISDKRepo _sdkRepo;
        private readonly ILogger _logger;
        public AddDocumentWithMovementSDKUseCase(ISDKRepo sdkRepo, ILogger logger)
        {
            _sdkRepo = sdkRepo;
            _logger = logger;
        }

        public async Task<DocumentDTO> Execute(DocumentDTO documento)
        {
            _logger.Log("Ejecutando caso de uso AddDocumentWithMovementSDKUseCase");

            var canWork = await _sdkRepo.StartTransaction();
            try
            {
                if (canWork)
                {
                    var documentStruct = documento.GetSDKDocumentStruct();
                    var movementStruct = documento.GetSDKMovementStruct();

                    var sqlDocument = await _sdkRepo.AddDocumentWithMovement(documentStruct, movementStruct);

                    _logger.Log($"Documento agregado con éxito. ID: {sqlDocument.CGUIDDOCUMENTO}");

                    if (NeedsExtraFields(documento))
                    {
                        try
                        {
                            await AddExtraFields(documento, sqlDocument.CIDDOCUMENTO);
                        }
                        catch (Exception ex)
                        {
                            _logger.Log($"Error al agregar campos extra al documento: {ex.Message}");
                        }
                    }

                    _sdkRepo.StopTransaction();

                    return new DocumentDTO(sqlDocument);
                }
                else
                {
                    _logger.Log("No se pudo iniciar la transacción para el caso de uso AddDocumentWithMovement.");
                    throw new SDKException("No se pudo iniciar la transacción para el caso de uso AddDocumentWithMovement.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log("Error en el caso de uso AddDocumentWithMovement.");
                throw new SDKException($"Error en el caso de uso AddDocumentWithMovement: {ex.Message}");
            }
            finally
            {
                _sdkRepo.StopTransaction();
            }
        }

        private bool NeedsExtraFields(DocumentDTO documento)
        {
            var ListAtributes = new List<string>()
            {
                documento.COBSERVACIONES,
                documento.CTEXTOEXTRA1,
                documento.CTEXTOEXTRA2,
                documento.CTEXTOEXTRA3,
            };

            foreach (var item in ListAtributes)
            {
                if (item != string.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task AddExtraFields(DocumentDTO documento, int idDocumento)
        {
            var dictColumnsToAdd = new Dictionary<string, string>();

            if (documento.COBSERVACIONES != string.Empty)
            {
                dictColumnsToAdd["COBSERVACIONES"] = documento.COBSERVACIONES;
            }
            if (documento.CTEXTOEXTRA1 != string.Empty)
            {
                dictColumnsToAdd["CTEXTOEXTRA1"] = documento.CTEXTOEXTRA1;
            }
            if (documento.CTEXTOEXTRA2 != string.Empty)
            {
                dictColumnsToAdd["CTEXTOEXTRA2"] = documento.CTEXTOEXTRA2;
            }
            if (documento.CTEXTOEXTRA3 != string.Empty)
            {
                dictColumnsToAdd["CTEXTOEXTRA3"] = documento.CTEXTOEXTRA3;
            }

            await _sdkRepo.SetDatoDocumento(dictColumnsToAdd, idDocumento);
        }
    }
}
