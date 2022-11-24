using Microsoft.EntityFrameworkCore;
using Practica.TiendasAPI.DbContexts;
using Practica.TiendasAPI.Entities;

namespace Practica.TiendasAPI.Services
{
    public class TiendaRepository : ITiendaRepository
    {   
        private readonly TiendaContext _context;
        public TiendaRepository(TiendaContext context)
        {
            _context = context ?? throw new ArgumentNullException();
        }

        public async Task AddProductoAsync(int shopId, Producto product)
        {
            var tienda = await GetTiendaAsync(shopId, false);
            if (tienda != null)
            {
                tienda.Productos.Add(product);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<Producto?> GetProductoAsync(int shopId, int productId)
        {
            return await _context.Productos
                .Where(x=>x.ShopId==shopId && x.Id == productId)
                .OrderBy(x=>x.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Producto>,PaginationMetadata)> GetProductosAsync(int shopId,
            int pageSize, int pageNumber, bool pagination = true)
        {

            var products = _context.Productos.Where(x => x.ShopId == shopId);

            if (pagination)
            {
                int totalItems = await products.CountAsync();
                var metadata = new PaginationMetadata(totalItems,
                                pageSize, pageNumber);

                var res = await products
                        .OrderBy(x => x.Name)
                        .Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize).ToListAsync();

                return (res, metadata);
            }
            else
            {
                int totalItems = await products.CountAsync();
                var metadata = new PaginationMetadata(totalItems,
                                totalItems, 1);

                var res = await products
                        .OrderBy(x => x.Name)
                        .ToListAsync();

                return (res, metadata);
            }

        }

        public Task<Tienda?> GetTiendaAsync(int id, bool includeProductos=true)
        {
            var tienda = _context.Tiendas.Where(x=>x.Id==id);
            if (includeProductos)
            {
                tienda = tienda.Include(x => x.Productos);
            }
            return tienda.FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Tienda>, PaginationMetadata)> GetTiendasAsync(string? filterBy,
            string? query, int pageSize, int pageNumber, bool pagination = true)
        {
            var tiendas = _context.Tiendas as IQueryable<Tienda>;
            if (!string.IsNullOrEmpty(filterBy))
            {
                tiendas = tiendas.Where(x => x.Name == filterBy);
            }
            if (!string.IsNullOrEmpty(query))
            {
                tiendas = tiendas.Where(x => x.Name.ToLower().Contains(query.ToLower()));
            }

            if (pagination) {
                int totalItems = await tiendas.CountAsync();
                var metadata = new PaginationMetadata(totalItems,
                                pageSize, pageNumber);

                var res = await tiendas
                        .Include(c => c.Productos)
                        .OrderBy(x => x.Name)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize).ToListAsync();

                return (res, metadata);
            }
            else
            {
                int totalItems = await tiendas.CountAsync();
                var metadata = new PaginationMetadata(totalItems,
                                totalItems, 1);

                var res = await tiendas
                        .Include(c => c.Productos)
                        .OrderBy(x => x.Name)
                        .ToListAsync();

                return (res, metadata);
            }
        }

        public void RemoveProducto(Producto product)
        {
            _context.Productos.Remove(product);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync()>=0;
        }

        public async Task<bool> TiendaExistsAsync(int shopId)
        {
            return await _context.Tiendas.AnyAsync(x=>x.Id==shopId);
        }
    }
}
