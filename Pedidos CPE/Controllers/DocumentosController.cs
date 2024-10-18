using Application.DTOs;
using Application.UseCases.SDK.Documentos;
using Domain.Entities.Estructuras;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class DocumentosController : Controller
    {
        private readonly AddDocumentSDKUseCase _addDocumentSDKUseCase;
        private readonly AddDocumentAndMovements _addDocumentAndMovements;
        public DocumentosController(AddDocumentSDKUseCase addDocumentSDKUseCase, AddDocumentAndMovements addDocumentAndMovements)
        {
            _addDocumentSDKUseCase = addDocumentSDKUseCase;
            _addDocumentAndMovements = addDocumentAndMovements;
        }

        [HttpPost]
        [Route("Documentos")]
        public async Task<ActionResult<ApiResponse>> PostDocument(DocumentDTO documento)
        {
            try
            {
                var documentDTO = await _addDocumentSDKUseCase.Execute(documento);
                return CreatedAtAction("PostDocument", new ApiResponse { Message = "Documento agregado con éxito", Data = documentDTO, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
