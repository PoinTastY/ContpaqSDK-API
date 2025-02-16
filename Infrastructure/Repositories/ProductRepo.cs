using Core.Domain.Entities.SQL;
using Core.Domain.Interfaces.Repositories.SQL;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepo : IProductoSQLRepo
    {
        private readonly ContpaqiSQLContext _context;
        private readonly DbSet<ProductoSQL> _productos;
        public ProductRepo(ContpaqiSQLContext contpaqiSQLContext) 
        {
            _context = contpaqiSQLContext;
            _productos = _context.Set<ProductoSQL>();
        }

        public async Task<IEnumerable<ProductoSQL>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _productos.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<ProductoSQL> GetByIdAsync(int idProducto, CancellationToken cancellationToken)
        {
            var product = await _productos.AsNoTracking().Where(
                p => p.CIDPRODUCTO == idProducto)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                throw new KeyNotFoundException($"No se encontro el producto con id: {idProducto}");
            
            return product;
        }

        public async Task<ProductoSQL> GetByCodigoAsync(string codigoProducto, CancellationToken cancellationToken)
        {
            var product = await _productos.AsNoTracking().Where(
                p => p.CCODIGOPRODUCTO == codigoProducto)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                throw new KeyNotFoundException($"No se encontro el producto con codigo: {codigoProducto}");
            
            return product;
        }

        public async Task<IEnumerable<ProductoSQL>> GetByIdsAsync(IEnumerable<int> idsProductos, CancellationToken cancellationToken)
        {
            //ESTA LA PUSE COMO CONSULTA RAW, PORQUE CON LINQ DABA ERROR AL PARSEAR LA LISTA, DECIA QUE TENIA UN CARACTER INVALIDO"$", NO SE XQ XD PERO ASI SIJALA
            if (idsProductos == null || !idsProductos.Any())
            {
                return new List<ProductoSQL>();
            }

            // Crea una cadena con los parámetros de la consulta
            var idList = string.Join(",", idsProductos);
            var query = $"SELECT * FROM admProductos WHERE CIDPRODUCTO IN ({idList}) AND CIDVALORCLASIFICACION6 != 0";

            // Ejecuta la consulta usando FromSqlRaw
            var productos = await _context.Set<ProductoSQL>()
                .FromSqlRaw(query)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (productos == null || !productos.Any())
                throw new KeyNotFoundException($"No se encontraron productos con los ids: {idList}");

            return productos;
        }

        public async Task<IEnumerable<ProductoSQL>> SearchByNameAsync(string name, CancellationToken cancellationToken)
        {
            var productos = await _productos.AsNoTracking().Where(
                p => p.CNOMBREPRODUCTO.Contains(name))
                .ToListAsync(cancellationToken);

            if (productos == null || !productos.Any())
                throw new KeyNotFoundException($"No se encontraron productos con el nombre: {name}");

            return productos;
        }

        public async Task<IEnumerable<ProductoSQL>> GetByMultipleCodigosAsync(IEnumerable<string> codigos, CancellationToken cancellationToken)
        {
            //ESTA LA PUSE COMO CONSULTA RAW, PORQUE CON LINQ DABA ERROR AL PARSEAR LA LISTA, DECIA QUE TENIA UN CARACTER INVALIDO"$", NO SE XQ XD PERO ASI SIJALA
            if (codigos == null || !codigos.Any())
            {
                return new List<ProductoSQL>();
            }

            // Crea una cadena con los parámetros de la consulta
            var codigosList = string.Join(",", codigos.Select(c => $"'{c}'"));
            var query = $"SELECT * FROM admProductos WHERE CCODIGOPRODUCTO IN ({codigosList})";

            // Ejecuta la consulta usando FromSqlRaw
            var productos = await _context.Set<ProductoSQL>()
                .FromSqlRaw(query)
                .AsNoTracking() // Para no rastrear los cambios
                .ToListAsync(cancellationToken);

            if (productos == null || !productos.Any())
                throw new KeyNotFoundException($"No se encontraron productos con los codigos: {codigosList}");

            return productos;
        }
    }
}
