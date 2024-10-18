using Application.DTOs;
using Application.UseCases.SDK.Documentos;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class DocumentosController : Controller
    {
        private readonly AddDocumentSDKUseCase _addDocumentSDKUseCase;
        public DocumentosController(AddDocumentSDKUseCase addDocumentSDKUseCase)
        {
            _addDocumentSDKUseCase = addDocumentSDKUseCase;
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
    }
}
