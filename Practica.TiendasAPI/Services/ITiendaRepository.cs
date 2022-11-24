
using Practica.TiendasAPI.Entities;

namespace Practica.TiendasAPI.Services
{
    public interface ITiendaRepository
    {

        Task<(IEnumerable<Tienda>, PaginationMetadata)> GetTiendasAsync(string? filterBy, string? query, int pageSize, int pageNumber, bool pagination = true);
        Task<Tienda?> GetTiendaAsync(int id, bool includeProductos);
        Task<(IEnumerable<Producto>, PaginationMetadata)> GetProductosAsync(int shopId, int pageSize, int pageNumber, bool pagination = true);
        Task<Producto?> GetProductoAsync(int shopId, int productId);
        Task<bool> TiendaExistsAsync(int shopId);
        Task AddProductoAsync(int shopId, Producto product);
        void RemoveProducto(Producto product);

        Task<bool> SaveChangesAsync();
    }
}
