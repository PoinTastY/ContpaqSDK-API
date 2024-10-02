using Application.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;
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
        public PatchMovimientoUnidadesByIdUseCase(ISDKRepo sDKRepo)
        {
            _sDKRepo = sDKRepo;
        }

        public async Task Execute(int idMovimiento, string unidades)
        {
            var canWork = await _sDKRepo.StartTransaction();
            if (canWork)
            {
                try
                {
                    await _sDKRepo.UpdateUnidadesMovimiento(idMovimiento, unidades);
                }
                catch (Exception ex)
                {
                    throw new SDKException($"Error en el caso de uso PatchMovimientoUnidadesById: {ex.Message}");
                }
                finally
                {
                    _sDKRepo.StopTransaction();
                }
            }
            throw new SDKException("No se pudo iniciar la transacción para el caso de uso PatchMovimientoUnidadesById.");
        }

        private void ThrowNotAvilable()
        {
            throw new Exception("Este entrypoint no funciona por el momento");
        }
    }
}
