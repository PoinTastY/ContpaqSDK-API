using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.DTOs;
using Core.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Postgres.Movimientos
{
    public class GetMovimientosByDocumentoIdPostgresUseCase
    {
        private readonly IMovimientoDtoRepo _movimientoRepo;
        private readonly ILogger _logger;

        public GetMovimientosByDocumentoIdPostgresUseCase(IMovimientoDtoRepo movimientoRepo, ILogger logger)
        {
            _movimientoRepo = movimientoRepo;
            _logger = logger;
        }

        public async Task<List<MovimientoDto>> Execute(int id)
        {
            try
            {
                _logger.Log("Obteniendo movimientos por id de documento de Postgres");
                return await _movimientoRepo.GetMovimientosByDocumentoIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error al obtener movimientos por id de documento de Postgres: {ex.Message} (Inner: {ex.InnerException})");
                throw new Exception($"Error al obtener movimientos por id de documento de Postgres: {ex.Message} (Inner: {ex.InnerException})");
            }
        }
    }
}
