using Domain.Entities;
using Domain.Interfaces.Repos;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly DbSet<ProductoSQL> _productos;
        public ProductRepo(ContpaqiSQLContext contpaqiSQLContext) 
        {
            _context = contpaqiSQLContext;
            _productos = _context.Set<ProductoSQL>();
        }

        public async Task<List<ProductoSQL>> GetAllProductsAsync()
        {
            return await _productos.AsNoTracking().ToListAsync();
        }

        public async Task<ProductoSQL> GetProductByIdAsync(int idProducto)
        {
            var product = await _productos.AsNoTracking().Where(p => p.CIDPRODUCTO == idProducto).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new NotFoundArgumentException($"No se encontro el producto con id: {idProducto}");
            }
            return product;
        }

        public async Task<ProductoSQL> GetProductByCodigoAsync(string codigoProducto)
        {
            var product = await _productos.AsNoTracking().Where(p => p.CCODIGOPRODUCTO == codigoProducto).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new NotFoundArgumentException($"No se encontro el producto con codigo: {codigoProducto}");
            }
            return product;
        }

        public async Task<List<ProductoSQL>> GetProductByIdsCPEAsync(IEnumerable<int> idsProductos)
        {
            return await _productos.AsNoTracking().Where(p => idsProductos.Contains(p.CIDPRODUCTO) && p.CIDVALORCLASIFICACION6 != 0).ToListAsync();
        }

        public async Task<List<ProductoSQL>> GetProductsByMultipleIdsAsync(IEnumerable<int> ids)
        {
            return await _productos.AsNoTracking().Where(p => ids.Contains(p.CIDPRODUCTO)).ToListAsync();
        }
    }
}
