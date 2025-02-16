using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SQL.Documentos
{
    public class GetPedidosSQLCPEUseCase
    {
        private readonly IDocumentoSQLRepo _documentRepo;
        private readonly ILogger _logger;

        public GetPedidosSQLCPEUseCase(IDocumentoSQLRepo documentRepo, ILogger logger)
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
            var documentos = await _documentRepo.GetAllDocumentsByFechaSerieCPEAsync(fechaInicio, fechaFin, serie);
            var dTOs = new List<DocumentDTO>();
            foreach (var documento in documentos)
            {
                dTOs.Add(new DocumentDTO(documento));
            }
            return dTOs;
        }
    }
}
