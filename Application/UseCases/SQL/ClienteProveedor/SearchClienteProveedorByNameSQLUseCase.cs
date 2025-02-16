using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.ClienteProveedor
{
    public class SearchClienteProveedorByNameSQLUseCase
    {
        private readonly IClienteProveedorSQLRepo _clienteProveedorRepo;
        private readonly ILogger _logger;
        public SearchClienteProveedorByNameSQLUseCase(ILogger logger, IClienteProveedorSQLRepo clienteProveedorRepo)
        {
            _clienteProveedorRepo = clienteProveedorRepo;
            _logger = logger;
        }

        public async Task<List<ClienteProveedorDTO>> Execute(string name)
        {
            _logger.Log($"Buscando clientes/proveedores: {name} por nombre en SQL");
            var clientesProveedores = await _clienteProveedorRepo.GetClienteProveedorByName(name);
            if (clientesProveedores.Count == 0)
            {
                _logger.Log($"No se encontraron clientes/proveedores con el nombre: {name}");
            }
            var clienteProveedorDTO = new List<ClienteProveedorDTO>();
            foreach (var clienteProveedor in clientesProveedores)
            {
                clienteProveedorDTO.Add(new ClienteProveedorDTO(clienteProveedor));
            }
            return clienteProveedorDTO;
        }
    }
}
