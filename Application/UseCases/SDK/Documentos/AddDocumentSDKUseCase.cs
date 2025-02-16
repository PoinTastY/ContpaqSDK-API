using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Exceptions;
using Core.Domain.Interfaces.Services;
using Core.Domain.Interfaces.Repositories.SQL;

namespace Application.UseCases.SDK.Documentos
{
    public class AddDocumentSDKUseCase
    {
        private readonly ISDKRepo _sdkRepo;
        private readonly ILogger _logger;
        public AddDocumentSDKUseCase(ISDKRepo sDKRepo, ILogger logger)
        {
            _logger = logger;
            _sdkRepo = sDKRepo;
        }

        public async Task<DocumentDTO> Execute(DocumentDTO documento)
        {
            _logger.Log("Ejecutando caso de uso AddDocumentSDKUseCase...");

            var canWork = await _sdkRepo.StartTransaction();
            try
            {
                if (canWork)
                {
                    var documentStruct = documento.GetSDKDocumentStruct();

                    var sqlDocument = await _sdkRepo.AddDocument(documentStruct);

                    documento.CIDDOCUMENTO = sqlDocument.CIDDOCUMENTO;

                    _logger.Log($"Documento agregado con éxito. ID: {documento.CIDDOCUMENTO}");

                    if (NeedsExtraFields(documento))
                    {
                        try
                        {
                            await AddExtraFields(documento, sqlDocument.CIDDOCUMENTO);
                            _logger.Log("Campos extra agregados correctamente al documento.");
                        }
                        catch (Exception ex)
                        {
                            _logger.Log($"Error al agregar campos extra al documento: {ex.Message} (Inner: {ex.InnerException})");
                        }
                    }
                    else
                    {
                        _logger.Log("No se requieren campos extra para el documento, y este fue creado correctamente, retornando...");
                    }
                    _sdkRepo.StopTransaction();

                    return documento;
                }
                else
                {
                    _logger.Log("No se pudo iniciar la transacción para el caso de uso AddDocumentWithMovement.");
                    throw new SDKException("No se pudo iniciar la transacción para el caso de uso AddDocumentWithMovement.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error en el caso de uso AddDocumentSDKUseCase: {ex.Message} (Inner: {ex.InnerException})");
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
