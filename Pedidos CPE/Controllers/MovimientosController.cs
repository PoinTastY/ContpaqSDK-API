using Application.DTOs;
using Application.UseCases.Postgres.Movimientos;
using Application.UseCases.SDK.Movimientos;
using Core.Domain.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class MovimientosController : Controller
    {
        private readonly UpdateMovimientosPostgresUseCase _updateMovimientos;
        private readonly GetMovimientosByDocumentoIdPostgresUseCase _getMovimientosByDocumentoIdPostgresUseCase;
        private readonly PatchMovimientoUnidadesByIdUseCase _patchMovimientoUnidadesByIdUseCase;

        public MovimientosController(UpdateMovimientosPostgresUseCase updateMovimientos, GetMovimientosByDocumentoIdPostgresUseCase getMovimientosByDocumentoIdPostgresUseCase,
                                        PatchMovimientoUnidadesByIdUseCase patchMovimientoUnidadesByIdUseCase)
        {
            _updateMovimientos = updateMovimientos;
            _getMovimientosByDocumentoIdPostgresUseCase = getMovimientosByDocumentoIdPostgresUseCase;
            _patchMovimientoUnidadesByIdUseCase = patchMovimientoUnidadesByIdUseCase;
        }

        [HttpPatch]
        [Route("Movimientos")]
        public async Task<ActionResult<ApiResponse>> PatchMovimientosUnidades(List<MovimientoDto> movimientos)
        {
            try
            {
                await _updateMovimientos.Execute(movimientos);
                return Ok(new ApiResponse { Message = "Movimientos agregados con éxito", Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Movimientos/ByDocumentoId")]
        public async Task<ActionResult<ApiResponse>> GetMovimientosByDocumentoId(int documentoId)
        {
            try
            {
                var movimientos = await _getMovimientosByDocumentoIdPostgresUseCase.Execute(documentoId);
                return Ok(new ApiResponse { Message = "Movimientos encontrados", Data = movimientos, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch]
        [Route("Movimientos/UpdateUnidades")]
        public async Task<ActionResult<ApiResponse>> PatchMovimientosUnidades([FromQuery] int idDocumentoPadre, [FromQuery] int idMovimiento, [FromQuery] int unitsToAdd)
        {
            try
            {
                await _patchMovimientoUnidadesByIdUseCase.Execute(idDocumentoPadre, idMovimiento, unitsToAdd.ToString());
                return Ok(new ApiResponse { Message = "Movimientos actualizados con éxito", Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
