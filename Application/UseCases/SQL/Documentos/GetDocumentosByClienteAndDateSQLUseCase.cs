using Application.DTOs;
using Core.Domain.Entities.SQL;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;
using Domain.Entities.Interfaces;
using Domain.Interfaces.Repos.PostgreRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SQL.Documentos
{
    public class GetDocumentosByClienteAndDateSQLUseCase
    {
        private readonly IDocumentoSQLRepo _documentoRepo;
        private readonly ILogger _logger;
        public GetDocumentosByClienteAndDateSQLUseCase(IDocumentoSQLRepo documentoRepo, ILogger logger)
        {
            _documentoRepo = documentoRepo;
            _logger = logger;
        }

        public async Task<List<DocumentDTO>> ExecuteAsync(int idCliente, DateTime fechaInicio, DateTime fechaFin)
        {
            _logger.Log("GetDocumentosByClienteAndDateUseCase called");
            List<DocumentoSQL> documents = await _documentoRepo.GetDocumentosByIdClienteAndDateAsync(idCliente, fechaInicio, fechaFin);

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
