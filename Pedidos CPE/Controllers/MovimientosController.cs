using Application.DTOs;
using Application.UseCases.SDK.Movimientos;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class MovimientosController : Controller
    {
        private readonly AddMovimientoSDKUseCase _addMovimientoSDKUseCase;
        public MovimientosController(AddMovimientoSDKUseCase addMovimientoSDKUseCase)
        {
            _addMovimientoSDKUseCase = addMovimientoSDKUseCase;
        }

        [HttpPost]
        [Route("Movimientos")]
        public async Task<ActionResult<ApiResponse>> PostMovimiento(MovimientoDTO movimiento)
        {
            try
            {
                var movimientoDTO = await _addMovimientoSDKUseCase.Execute(movimiento);
                return CreatedAtAction("PostMovimiento", new ApiResponse { Message = "Movimiento agregado con éxito", Data = movimientoDTO, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
