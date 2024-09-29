using Application.DTOs;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Documentos
{
    public class GetPedidosSQLUseCase
    {
        private readonly IDocumentRepo _documentRepo;
        private readonly ILogger _logger;

        public GetPedidosSQLUseCase(IDocumentRepo documentRepo, ILogger logger)
        {
            _documentRepo = documentRepo;
            _logger = logger;
        }

        /// <summary>
        /// Gets the documents by fecha and serie
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="serie"></param>
        /// <returns></returns>
        public async Task<List<DocumentDTO>> Execute(DateTime fechaInicio, DateTime fechaFin, string serie)
        {
            _logger.Log("GetPedidosCPESQLUseCase called");
            var documentos = await _documentRepo.GetAllDocumentsByFechaAndSerieAsync(fechaInicio, fechaFin, serie);
            var dTOs = new List<DocumentDTO>();
            foreach (var documento in documentos)
            {
                dTOs.Add(new DocumentDTO(documento));
            }
            return dTOs;
        }
    }
}
