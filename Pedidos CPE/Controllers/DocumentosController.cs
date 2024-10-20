﻿using Application.DTOs;
using Application.UseCases.Postgres;
using Application.UseCases.SDK.Documentos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class DocumentosController : Controller
    {
        private readonly AddDocumentAndMovementsSDKUseCase _addDocumentAndMovements;
        private readonly AddDocumentAndMovementsPostgresUseCase _addDocumentAndMovementsPostgres;
        private readonly GetDocumentosPendientesUseCase _getDocumentosPendientes;
        private readonly Domain.Interfaces.Services.ILogger _logger;
        public DocumentosController(Domain.Interfaces.Services.ILogger logger,AddDocumentAndMovementsSDKUseCase addDocumentAndMovements, AddDocumentAndMovementsPostgresUseCase addDocumentAndMovementsPostgresUseCase
            , GetDocumentosPendientesUseCase getDocumentosPendientes)
        {
            _addDocumentAndMovements = addDocumentAndMovements;
            _addDocumentAndMovementsPostgres = addDocumentAndMovementsPostgresUseCase;
            _logger = logger;
            _getDocumentosPendientes = getDocumentosPendientes;
        }

        [HttpPost]
        [Route("DocumentosSDK")]
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
    }
}
