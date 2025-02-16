using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Movimientos
{
    public class GetMovimientosByIdDocumentoSQLUseCase
    {
        private readonly IMovimientoSQLRepo _movimientoRepo;
        private readonly ILogger _logger;

        public GetMovimientosByIdDocumentoSQLUseCase(IMovimientoSQLRepo movimientoRepo, ILogger logger)
        {
            _movimientoRepo = movimientoRepo;
            _logger = logger;
        }

        /// <summary>
        /// Asks for the movements of a document by its id
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns>List of MovimientoDTO instances</returns>
        public async Task<List<MovimientoDTO>> Execute(int idDocumento)
        {
            _logger.Log("GetMovimientosByIdDocumentoUseCase called");
            var movimientos = await _movimientoRepo.GetMovimientosByDocumentId(idDocumento);

            var dTOs = new List<MovimientoDTO>();

            foreach (var movimiento in movimientos)
            {
                dTOs.Add(new MovimientoDTO(movimiento));
            }

            return dTOs;
        }
    }
}
