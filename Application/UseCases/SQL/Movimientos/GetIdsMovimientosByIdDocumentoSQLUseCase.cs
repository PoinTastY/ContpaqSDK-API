using Domain.Interfaces.Repos;
using Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Movimientos
{
    public class GetIdsMovimientosByIdDocumentoSQLUseCase
    {
        private readonly IMovimientoRepo _movimientoRepo;
        private readonly ILogger _logger;
        public GetIdsMovimientosByIdDocumentoSQLUseCase(IMovimientoRepo movimientoRepo, ILogger logger)
        {
            _movimientoRepo = movimientoRepo;
            _logger = logger;
        }

        /// <summary>
        /// Asks for the list of movement ID's by document ID
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns>movements IDs ONLY</returns>
        public async Task<List<int>> Execute(int idDocumento)
        {
            _logger.Log("GetIdsMovimientosByIdDocumentoUseCase called");
            return await _movimientoRepo.GetMovimientosIdsByDocumenId(idDocumento);
        }
    }
}
