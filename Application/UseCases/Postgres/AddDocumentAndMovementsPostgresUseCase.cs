using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;
using Core.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Postgres
{
    public class AddDocumentAndMovementsPostgresUseCase
    {
        private readonly IDocumentoDtoRepo _documentoRepo;
        private readonly ILogger _logger;

        public AddDocumentAndMovementsPostgresUseCase(IDocumentoDtoRepo documentoRepo, ILogger logger)
        {
            _documentoRepo = documentoRepo;
            _logger = logger;
        }

        public async Task<int> Execute(DocumentoDto documento, List<MovimientoDto> movimientos)
        {
            _logger.Log("Ejecutando caso de uso AddDocumentAndMovementsPostgresUseCase...");
            var id = await _documentoRepo.AddDocumentoAndMovimientoAsync(documento, movimientos);
            _logger.Log($"Documento agregado con éxito. ID: {id}");
            return id;
        }
    }
}
