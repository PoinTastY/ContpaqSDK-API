using Application.DTOs;
using Application.UseCases.SDK.Documentos;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class DocumentosController : Controller
    {
        private readonly AddDocumentAndMovementsSDKUseCase _addDocumentAndMovements;
        public DocumentosController(AddDocumentAndMovementsSDKUseCase addDocumentAndMovements)
        {
            _addDocumentAndMovements = addDocumentAndMovements;
        }

        [HttpPost]
        [Route("Documentos/Movimientos")]
        public async Task<ActionResult<ApiResponse>> PostDocumentAndMovements(DocumentoConMovimientosDTO request)
        {
            try
            {
                var documentDTO = await _addDocumentAndMovements.Execute(request);
                return CreatedAtAction("PostDocumentAndMovements", new ApiResponse { Message = "Documento y movimientos agregados con éxito", Data = documentDTO, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
