using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Practica.TiendasAPI;
using Practica.TiendasAPI.DbContexts;
using Practica.TiendasAPI.Entities;
using Practica.TiendasAPI.Services;

namespace Practica.TiendasAPI.Tests.RepositoryTests
{
    public class TiendaRepositoryTest: IDisposable
    {
        private TiendaRepository _tiendaRepository;
        public TiendaRepositoryTest()
        {
            var connection = new SqliteConnection("Data source=:memory:");
            connection.Open();

            var optionsBuilder = new DbContextOptionsBuilder<TiendaContext>().UseSqlite(connection);
            var dbContext = new TiendaContext(optionsBuilder.Options);
            dbContext.Database.Migrate();

            _tiendaRepository = new TiendaRepository(dbContext);
        }
        public void Dispose()
        {
        }

        [Fact]
        public async Task GetTiendas_CountResults()
        {
            var (shops,_) = await _tiendaRepository.GetTiendasAsync(null,null,10,1);

            Assert.NotNull(shops);
            Assert.Equal(4, shops.Count());
        }

        [Fact]
        public async Task GetTiendas_Pagination()
        {
            int pageNumber = 1;
            int pageSize = 2;
            var (shops, metadata) = await _tiendaRepository.GetTiendasAsync(null, null, pageSize, pageNumber);

            Assert.NotNull(shops);
            Assert.Equal(2, metadata.TotalPageCount);
            Assert.Equal(4, metadata.TotalItemCount);
            Assert.Equal(pageNumber, metadata.CurrentPage);
            Assert.Equal(pageSize, shops.Count());
        }

        [Fact]
        public async Task GetTiendas_FilterBy()
        {
            string filterQuery = "Panadería Paqui";
            var (shops, _) = await _tiendaRepository.GetTiendasAsync(filterQuery, null, 10, 1);

            Assert.NotNull(shops);
            Assert.Single(shops);
            Assert.Equal("Panadería Paqui", shops.First().Name);
        }

        [Fact]
        public async Task GetTiendas_SearchQuery()
        {
            string searchQuery = "Pan";
            var (shops, _) = await _tiendaRepository.GetTiendasAsync(null, searchQuery, 10, 1);

            Assert.NotNull(shops);
            Assert.Single(shops);
            Assert.Equal("Panadería Paqui", shops.First().Name);
        }

        [Fact]
        public async Task GetTienda_NoProducts()
        {
            int shopId = 1;

            var shop = await _tiendaRepository.GetTiendaAsync(shopId,false);

            Assert.NotNull(shop);
            Assert.Empty(shop.Productos);
            Assert.Equal("Panadería Paqui", shop.Name);
        }
        [Fact]
        public async Task GetTienda_TiendaDontExist()
        {
            int shopId = 99;

            var shop = await _tiendaRepository.GetTiendaAsync(shopId);

            Assert.Null(shop);
        }

        [Fact]
        public async Task GetTienda_WithProducts()
        {
            int shopId = 1;
            var shop = await _tiendaRepository.GetTiendaAsync(shopId, true);

            Assert.NotNull(shop);
            Assert.NotEmpty(shop.Productos);
            Assert.Equal("Panadería Paqui", shop.Name);
        }

        [Fact]
        public async Task GetProductos_CountResults()
        {
            int shopId = 1;

            var (products, _) = await _tiendaRepository.GetProductosAsync(shopId, 10, 1);

            Assert.NotNull(products);
            Assert.Equal(2, products.Count());
        }

        [Fact]
        public async Task GetProductos_Pagination()
        {
            int shopId = 1;
            int pageNumber = 1;
            int pageSize = 1;
            var (products, metadata) = await _tiendaRepository.GetProductosAsync(shopId, pageSize, pageNumber);

            Assert.NotNull(products);
            Assert.Equal(2, metadata.TotalPageCount);
            Assert.Equal(2, metadata.TotalItemCount);
            Assert.Equal(pageNumber, metadata.CurrentPage);
            Assert.Equal(pageSize, products.Count());
        }

        [Fact]
        public async Task GetProductos_TiendaDontExist()
        {
            int shopId = 99;

            var (products, _) = await _tiendaRepository.GetProductosAsync(shopId, 10, 1);

            Assert.Empty(products);
        }

        [Fact]
        public async Task GetProducto_Success()
        {
            int shopId = 1;
            int productId = 1;

            var product = await _tiendaRepository.GetProductoAsync(shopId, productId);

            Assert.NotNull(product);
            Assert.Equal("Pan integral", product.Name);
        }

        [Fact]
        public async Task GetProducto_ProductDontExist()
        {
            int shopId = 1;
            int productId = 99;

            var product = await _tiendaRepository.GetProductoAsync(shopId, productId);

            Assert.Null(product);
        }

        [Fact]
        public async Task TiendaExists_CheckTiendaExists()
        {
            var shopIdExists = 1;
            var shopIdNotExists = 99;

            Assert.True(await _tiendaRepository.TiendaExistsAsync(shopIdExists));
            Assert.False(await _tiendaRepository.TiendaExistsAsync(shopIdNotExists));
        }
        [Fact]
        public async Task CreateProduct_Success()
        {
            int shopId = 1;
            Producto newProduct = new Producto()
            {
                Name = "Pan Bimbo",
                Description = "Rebanadas de pan Bimbo."
            };
            await _tiendaRepository.AddProductoAsync(shopId, newProduct);
            var (products, _) = await _tiendaRepository.GetProductosAsync(shopId, 10, 1);

            Assert.Equal(2,products.Count());

            // Guarda los cambios y comprueba que se añade un nuevo producto.
            await _tiendaRepository.SaveChangesAsync();
            var (productsUpdated, _) = await _tiendaRepository.GetProductosAsync(shopId, 10, 1);
            Assert.Equal(3, productsUpdated.Count());

        }

        [Fact]
        public async Task DeleteProduct_Success()
        {
            int shopId = 1;
            int productId = 1;

            Producto? productToDelete = await _tiendaRepository.GetProductoAsync(shopId, productId);
            _tiendaRepository.RemoveProducto(productToDelete);

            // Aún no está borrado asi que no devuelve null
            Producto? product = await _tiendaRepository.GetProductoAsync(shopId, productId);
            Assert.NotNull(product);

            // Guarda los cambios y comprueba que ahora es null
            await _tiendaRepository.SaveChangesAsync();
            Producto? deletedProduct = await _tiendaRepository.GetProductoAsync(shopId, productId);
            Assert.Null(deletedProduct);

        }
    }
}
