using Domain.Interfaces.Repos.PostgreRepo;
using Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Postgres
{
    public class MovimientoRepo : IMovimientoRepo
    {
        private readonly DbSet<Movimiento> _movimientos;
        private readonly PostgresCPEContext _dbContext;

        public MovimientoRepo(PostgresCPEContext dbContext)
        {
            _movimientos = dbContext.Set<Movimiento>();
            _dbContext = dbContext;
        }

        public async Task AddMovimientosAsync(List<Movimiento> movimientos)
        {
            await _dbContext.AddRangeAsync(movimientos);
        }
    }
}
