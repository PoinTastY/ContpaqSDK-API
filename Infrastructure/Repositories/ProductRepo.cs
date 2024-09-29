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

        public async Task<List<ProductoSQL>> GetProductByIdsCPEAsync(List<int> idsProductos)
        {
            return await _productos.AsNoTracking().Where(p => idsProductos.Contains(p.CIDPRODUCTO) && p.CIDVALORCLASIFICACION6 != 0).ToListAsync();
        }

        public async Task<List<ProductoSQL>> GetProductsByMultipleIdsAsync(List<int> ids)
        {
            //ESTA LA PUSE COMO CONSULTA RAW, PORQUE CON LINQ DABA ERROR AL PARSEAR LA LISTA, DECIA QUE TENIA UN CARACTER INVALIDO"$", NO SE XQ XD PERO ASI SIJALA
            if (ids == null || !ids.Any())
            {
                return new List<ProductoSQL>();
            }

            // Crea una cadena con los parámetros de la consulta
            var idList = string.Join(",", ids);
            var query = $"SELECT * FROM admProductos WHERE CIDPRODUCTO IN ({idList})";

            // Ejecuta la consulta usando FromSqlRaw
            var productos = await _context.Set<ProductoSQL>()
                .FromSqlRaw(query)
                .AsNoTracking() // Para no rastrear los cambios
                .ToListAsync();

            return productos;
        }
    }
}
