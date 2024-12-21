using Application.DTOs;
using Domain.Entities.Estructuras;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SDK.Movimientos
{
    public class PatchMovimientoUnidadesByIdUseCase
    {
        private readonly ISDKRepo _sDKRepo;
        private readonly ILogger _logger;
        public PatchMovimientoUnidadesByIdUseCase(ISDKRepo sDKRepo, ILogger logger)
        {
            _sDKRepo = sDKRepo;
            _logger = logger;
        }

        public async Task Execute(int idDocumentoPadre, int idMovimiento, string unidades)
        {
            var canWork = await _sDKRepo.StartTransaction();
            if (canWork)
            {
                try
                {
                    _logger.Log("Iniciando caso de uso PatchMovimientoUnidadesById");
                    await _sDKRepo.UpdateUnidadesMovimiento(idDocumentoPadre, idMovimiento, unidades);
                    _logger.Log("Unidades actualizadas con éxito");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.Log($"Error en el caso de uso PatchMovimientoUnidadesById: {ex.Message}");
                    throw new SDKException($"Error en el caso de uso PatchMovimientoUnidadesById: {ex.Message}");
                }
                finally
                {
                    _logger.Log("Finalizando caso de uso PatchMovimientoUnidadesById");
                    _sDKRepo.StopTransaction();
                }
            }
            _logger.Log("No se pudo iniciar la transacción para el caso de uso PatchMovimientoUnidadesById.");
            throw new SDKException("No se pudo iniciar la transacción para el caso de uso PatchMovimientoUnidadesById.");
        }

        private void ThrowNotAvilable()
        {
            throw new Exception("Este entrypoint no funciona por el momento");
        }
    }
}
