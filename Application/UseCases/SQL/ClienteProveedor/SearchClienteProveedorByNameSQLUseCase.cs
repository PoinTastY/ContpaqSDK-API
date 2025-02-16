using Application.DTOs;
using Core.Domain.Entities.SQL;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.ClienteProveedor
{
    public class SearchClienteProveedorByNameSQLUseCase
    {
        private readonly IClienteProveedorSQLRepo _clienteProveedorSQLRepo;
        private readonly ILogger _logger;
        public SearchClienteProveedorByNameSQLUseCase(ILogger logger, IClienteProveedorSQLRepo clienteProveedorSQLRepo)
        {
            _clienteProveedorSQLRepo = clienteProveedorSQLRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<ClienteProveedorDto>> Execute(string name)
        {
            _logger.Log($"Buscando clientes/proveedores similares a: {name}");

            var clientesProveedores = await _clienteProveedorSQLRepo.SearchByName(name);

            return clientesProveedores.Select(c => new ClienteProveedorDto(c));
        }
    }
}
