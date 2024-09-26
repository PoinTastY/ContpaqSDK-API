﻿using Application.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class GetDocumentByIdUseCase
    {
        private readonly ISDKRepo _sDKRepo;
        private readonly ILogger _logger;
        public GetDocumentByIdUseCase(ISDKRepo sDKRepo, ILogger logger)
        {
            _logger = logger;
            _sDKRepo = sDKRepo;
        }

        public async Task<DocumentDTO> Execute(int idDocumento)
        {
            var canWork = await _sDKRepo.StartTransaction();
            if (canWork)
            {
                try
                {

                    var document = await _sDKRepo.GetDocumentoById(idDocumento);

                    var dto = new DocumentDTO(document, idDocumento);

                    return dto;
                }
                catch (Exception ex)
                {
                    _logger.Log("Error en el caso de uso GetDocumentById.");
                    throw new SDKException($"Error en el caso de uso GetDocumentById: {ex.Message}");
                }
                finally
                {
                    _sDKRepo.StopTransaction();
                }
            }
            throw new SDKException("No se pudo iniciar la transacción para el caso de uso GetDocumentById.");
        }
    }
}
