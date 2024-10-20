using Domain.Entities;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ClienteProveedorRepo : IClienteProveedorRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly ILogger _logger;
        private readonly DbSet<ClienteProveedorSQL> _clientesProveedores;
        public ClienteProveedorRepo(ContpaqiSQLContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            _clientesProveedores = context.Set<ClienteProveedorSQL>();
        }

        public async Task<List<ClienteProveedorSQL>> GetClienteProveedorByName(string name)
        {
            return await _clientesProveedores.AsNoTracking().Where(cp => cp.CRAZONSOCIAL.Contains(name)).ToListAsync();
        }
    }
}
