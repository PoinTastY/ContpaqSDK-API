using Core.Domain.Entities.SQL;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.UseCases.SQL.Documentos
{
    public class GetDocumentosByIdClienteAndDateSQLUseCase
    {
        private readonly IDocumentoSQLRepo _documentoSQLRepo;
        private readonly ILogger _logger;
        public GetDocumentosByIdClienteAndDateSQLUseCase(IDocumentoSQLRepo documentoSQLRepo, ILogger logger)
        {
            _documentoSQLRepo = documentoSQLRepo;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista de documentos por id de cliente y rango de fechas
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public async Task<IEnumerable<DocumentoSQL>> Execute(int idCliente, DateTime fechaInicio, DateTime fechaFin)
        {
            _logger.Log("Ejecutando caso de uso GetDocumentosByIdClienteAndDateSQLUseCase...");

            var documentos = await _documentoSQLRepo.GetByIdClienteAndDateAsync(idCliente, fechaInicio, fechaFin);


            _logger.Log($"Se encontraron {documentos.Count()} documentos para el cliente {idCliente} entre las fechas: {fechaInicio} y {fechaFin}");

            return documentos;
        }
    }
}
