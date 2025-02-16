using Core.Domain.Entities.SQL;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ClienteProveedorRepo : IClienteProveedorSQLRepo
    {
        private readonly DbSet<ClienteProveedorSQL> _clientesProveedores;
        public ClienteProveedorRepo(ContpaqiSQLContext context)
        {
            _clientesProveedores = context.clientesProveedores;
        }

        public async Task<IEnumerable<ClienteProveedorSQL>> SearchByName(string name, CancellationToken cancellationToken)
        {
            var clientesProveedores = await _clientesProveedores.AsNoTracking().Where(
                cp => cp.CRAZONSOCIAL.Contains(name))
                .ToListAsync(cancellationToken);

            if (clientesProveedores.Count == 0)
                throw new KeyNotFoundException($"No se encontraron clientes/proveedores con el nombre: {name}");

            return clientesProveedores;
        }
    }
}
