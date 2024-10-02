using Domain.Interfaces.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SQL.Movimientos
{
    public class PatchUnidadesMovimientoByIdSQLUseCase
    {
        private readonly IMovimientoRepo _movimientoRepo;
        public PatchUnidadesMovimientoByIdSQLUseCase(IMovimientoRepo movimientoRepo)
        {
            _movimientoRepo = movimientoRepo;
        }

        public async Task Execute(int idMovimiento, double unidades)
        {
            await _movimientoRepo.UpdateUnidadesMovimientoById(idMovimiento, unidades);
        }
    }
}
