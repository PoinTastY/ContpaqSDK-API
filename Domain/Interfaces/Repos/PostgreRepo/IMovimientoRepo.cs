using Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repos.PostgreRepo
{
    public interface IMovimientoRepo
    {
        /// <summary>
        /// Adds movimientos to the database
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        Task AddMovimientosAsync(List<Movimiento> movimientos);
    }
}
