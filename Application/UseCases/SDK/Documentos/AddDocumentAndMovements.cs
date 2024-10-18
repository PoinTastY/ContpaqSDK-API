using Application.DTOs;
using Domain.Entities.Estructuras;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SDK.Documentos
{
    public class AddDocumentAndMovements
    {
        private readonly ISDKRepo _sdkRepo;
        private readonly ILogger _logger;

        public AddDocumentAndMovements(ISDKRepo sDKRepo, ILogger logger)
        {
            _logger = logger;
            _sdkRepo = sDKRepo;
        }

        public async Task<DocumentDTO> Execute(DocumentoConMovimientos request)
        {
            _logger.Log("Ejecutando caso de uso AddDocumentAndMovements...");

            var canWork = await _sdkRepo.StartTransaction();
            try
            {
                if (canWork)
                {
                    var documentoDTO = await _sdkRepo.AddDocumentAndMovements(request.Documento, request.Movimientos);

                    _logger.Log($"Documento agregado con éxito. ID: {documentoDTO.CIDDOCUMENTO}");

                    _sdkRepo.StopTransaction();

                    return new DocumentDTO(documentoDTO);
                }
                else
                {
                    _logger.Log("No se pudo iniciar la transacción para el caso de uso AddDocumentAndMovements.");
                    throw new SDKException("No se pudo iniciar la transacción para el caso de uso AddDocumentAndMovements.");
                }
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
