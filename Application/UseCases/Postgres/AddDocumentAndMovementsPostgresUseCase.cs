using Domain.Entities.Interfaces;
using Domain.Interfaces.Repos.PostgreRepo;
using Domain.Interfaces.Services;
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
        private readonly IDocumentoRepo _documentoRepo;
        private readonly ILogger _logger;

        public AddDocumentAndMovementsPostgresUseCase(IDocumentoRepo documentoRepo, ILogger logger)
        {
            _documentoRepo = documentoRepo;
            _logger = logger;
        }

        public async Task<int> Execute(Documento documento, List<Movimiento> movimientos)
        {
            _logger.Log("Ejecutando caso de uso AddDocumentAndMovementsPostgresUseCase...");
            var id = await _documentoRepo.AddDocumentoAndMovimientoAsync(documento, movimientos);
            _logger.Log($"Documento agregado con éxito. ID: {id}");
            return id;
        }
    }
}
