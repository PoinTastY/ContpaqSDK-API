using Domain.Entities;

namespace Domain.Interfaces.Repos
{
    public interface IClienteProveedorRepo
    {
        Task<List<ClienteProveedorSQL>> GetClienteProveedorByName(string name);
    }
}
