using Application.DTOs;
using Application.UseCases.Postgres;
using Application.UseCases.SDK;
using Application.UseCases.SDK.Documentos;
using Application.UseCases.SQL.Documentos;
using Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class DocumentosController : Controller
    {
        private readonly AddDocumentAndMovementsSDKUseCase _addDocumentAndMovementsSDK;
        //staremedic
        private readonly AddDocumentWithMovementSDKUseCase _addDocumentWithMovementSDK;
        private readonly SetDocumentoImpresoSDKUseCase _setDocumentoImpresoSDK;
        private readonly TestSDKUseCase _testSDKUseCase;

        private readonly AddDocumentAndMovementsPostgresUseCase _addDocumentAndMovementsPostgres;
        private readonly GetDocumentosPendientesUseCase _getDocumentosPendientes;
        private readonly UpdateDocumentoPendientePostgresUseCase _updateDocumentoPendienteUseCase;
        private readonly GetDocumentosByClienteAndDateSQLUseCase _getDocumentosByClienteAndDateSQLUseCase;
        private readonly Domain.Interfaces.Services.ILogger _logger;
        public DocumentosController(Domain.Interfaces.Services.ILogger logger, AddDocumentAndMovementsSDKUseCase addDocumentAndMovementsSDK, AddDocumentAndMovementsPostgresUseCase addDocumentAndMovementsPostgresUseCase
            , GetDocumentosPendientesUseCase getDocumentosPendientes, UpdateDocumentoPendientePostgresUseCase updateDocumentoPendienteUseCase
            , GetDocumentosByClienteAndDateSQLUseCase getDocumentosByClienteAndDateSQLUseCase, AddDocumentWithMovementSDKUseCase addDocumentWithMovementSDK
            , SetDocumentoImpresoSDKUseCase setDocumentoImpresoSDKUseCase, TestSDKUseCase testSDKUseCase)
        {
            _addDocumentAndMovementsSDK = addDocumentAndMovementsSDK;
            _addDocumentAndMovementsPostgres = addDocumentAndMovementsPostgresUseCase;
            _logger = logger;
            _getDocumentosPendientes = getDocumentosPendientes;
            _updateDocumentoPendienteUseCase = updateDocumentoPendienteUseCase;
            _getDocumentosByClienteAndDateSQLUseCase = getDocumentosByClienteAndDateSQLUseCase;
            _addDocumentWithMovementSDK = addDocumentWithMovementSDK;
            _setDocumentoImpresoSDK = setDocumentoImpresoSDKUseCase;
            _testSDKUseCase = testSDKUseCase;
        }

        [HttpPost]
        [Route("DocumentosSDK")]
        public async Task<ActionResult<ApiResponse>> PostDocumentAndMovements(DocumentoConMovimientosDTO request)
        {
            try
            {
                var documentDTO = await _addDocumentAndMovementsSDK.Execute(request);
                return CreatedAtAction("PostDocumentAndMovements", new ApiResponse { Message = "Documento y movimientos agregados con éxito", Data = documentDTO, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #region StareMedic

        [HttpPost]
        [Route("/addDocumentWithMovementSDK/")]
        public async Task<ActionResult<Pedidos_CPE.Controllers.Response.Response>> AddDocumentWithMovementSDK(DocumentDTO documento)
        {
            try
            {
                var documentDTO = await _addDocumentWithMovementSDK.Execute(documento);
                var response = new Pedidos_CPE.Controllers.Response.Response
                {
                    FolioDocumento = documentDTO.aFolio,
                    Message = "Se envió la remisión con éxito a Contpaqi Comercia",
                    IdDocumento = documentDTO.CIDDOCUMENTO
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new Pedidos_CPE.Controllers.Response.Response
                {
                    FolioDocumento = 0, 
                    Message = ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPut]
        [Route("/setDocumentoImpresoSDK/")]
        public async Task<ActionResult<ApiResponse>> SetDocumentoImpresoSDK([FromQuery] int idDocumento)
        {
            try
            {
                await _setDocumentoImpresoSDK.Execute(idDocumento);
                return Ok(new ApiResponse { Message = "Documento actualizado con éxito", Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("/isServiceWorkingSDK")]
        public async Task<ActionResult<ApiResponse>> IsServiceWorkingSDK()
        {
            try
            {
                await _testSDKUseCase.Execute();
                return Ok(new ApiResponse { Message = "ContpaqSDK-API is working", Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        [HttpPost]
        [Route("/Pendientes")]
        public async Task <ActionResult<ApiResponse>> PostDocumentAndMovementsPostgres(DocumentoConMovimientosPostgresDTO request)
        {
            try
            {
                if(request.Documento.IdInterfaz > 0)
                {
                    _logger.Log("Documento con ID de interfaz ya proporcionado, no se puede agregar a Postgres");
                    throw new Exception("Documento con ID de interfaz ya proporcionado, no se puede agregar a Postgres");
                }
                var idDocumento = await _addDocumentAndMovementsPostgres.Execute(request.Documento, request.Movimientos);
                return CreatedAtAction(nameof(PostDocumentAndMovementsPostgres), new ApiResponse { Message = "Documento y movimientos agregados con éxito", Data = idDocumento, Success = true });
            }
            catch (Exception ex)
            {
                _logger.Log($"Error al agregar documento y movimientos a Postgres: {ex.Message} (Inner: {ex.InnerException})");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + $" (Inner: {ex.InnerException})");
            }
        }

        [HttpGet]
        [Route("/Pendientes")]
        public async Task<ActionResult<ApiResponse>> GetDocumentosPendientes()
        {
            try
            {
                var documentos = await _getDocumentosPendientes.Execute();
                return Ok(new ApiResponse { Message = "Documentos pendientes obtenidos con éxito", Data = documentos, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("/SQL/ByClienteAndDate/")]
        public async Task<ActionResult<ApiResponse>> GetDocumentosByIdAndDate([FromQuery] int idCliente, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                var documentos = await _getDocumentosByClienteAndDateSQLUseCase.ExecuteAsync(idCliente, fechaInicio, fechaFin);
                return Ok(new ApiResponse { Message = "Documentos pendientes obtenidos con éxito", Data = documentos, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("/Pendientes")]
        public async Task<ActionResult<ApiResponse>> UpdateDocumentoPendiente(Documento request)
        {
            try
            {
                await _updateDocumentoPendienteUseCase.Execute(request);
                return Ok(new ApiResponse { Message = "Documento actualizado con éxito", Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
