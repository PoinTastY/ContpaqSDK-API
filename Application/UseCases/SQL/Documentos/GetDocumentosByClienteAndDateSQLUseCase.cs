using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Interfaces;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Repos.PostgreRepo;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SQL.Documentos
{
    public class GetDocumentosByClienteAndDateSQLUseCase
    {
        private readonly IDocumentRepo _documentoRepo;
        private readonly ILogger _logger;
        public GetDocumentosByClienteAndDateSQLUseCase(IDocumentRepo documentoRepo, ILogger logger)
        {
            _documentoRepo = documentoRepo;
            _logger = logger;
        }

        public async Task<List<DocumentDTO>> ExecuteAsync(int idCliente, DateTime fechaInicio, DateTime fechaFin)
        {
            _logger.Log("GetDocumentosByClienteAndDateUseCase called");
            List<DocumentSQL> documents = await _documentoRepo.GetDocumentosByIdClienteAndDateAsync(idCliente, fechaInicio, fechaFin);

            List<DocumentDTO> documentsDTO = new();

            foreach (var document in documents)
            {
                documentsDTO.Add(new DocumentDTO(document));
            }

            _logger.Log($"Found {documentsDTO.Count} documents for client {idCliente} between {fechaInicio} and {fechaFin}");

            return documentsDTO;
        }
    }
}
