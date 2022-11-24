using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Practica.TiendasAPI.Controllers;
using Practica.TiendasAPI.Entities;
using Practica.TiendasAPI.Models;
using Practica.TiendasAPI.Profiles;
using Practica.TiendasAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Practica.TiendasAPI.Tests.ControllerTests
{
    public class ProductoControllerTests
    {
        private ProductoController _productoController;
        private HttpContext _httpContext;

        public ProductoControllerTests()
        {
            var MockLogger = new Mock<ILogger<ProductoController>>();
            var MockRepository = new Mock<ITiendaRepository>();

            var mapperConf = new MapperConfiguration(cfg => cfg.AddProfile<ProductoProfile>());
            var mapper = new Mapper(mapperConf);

            _httpContext = new DefaultHttpContext();

            MockRepository.Setup(m => m.GetTiendaAsync(1, It.IsAny<bool>()))
                    .ReturnsAsync(new Tienda() { Name = "Tienda A" });
            MockRepository.Setup(m => m.TiendaExistsAsync(1))
                    .ReturnsAsync(true);
            MockRepository.Setup(m => m.TiendaExistsAsync(99))
                    .ReturnsAsync(false);
            MockRepository.Setup(m => m.GetProductosAsync(1,10,1,true))
                    .ReturnsAsync((new List<Producto>() {
                        new Producto() { Name = "Producto A" },
                        new Producto() { Name = "Producto B" }},
                        new PaginationMetadata(2,10,1)));
            MockRepository.Setup(m => m.GetProductoAsync(1, 1))
                    .ReturnsAsync(new Producto() { Name = "Producto A" });

            _productoController = new ProductoController(MockLogger.Object, MockRepository.Object, mapper);

            _productoController.ControllerContext = new ControllerContext()
            {
                HttpContext = _httpContext,
            };
        }
        [Fact]
        public async void GetProductos_OkResult_CountResults()
        {
            var res = await _productoController.GetProducts(1);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductoDto>>>(res);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<ProductoDto>>(okResult.Value);
            Assert.Equal(2, dtos.Count());
            Assert.Equal("Producto A", dtos.First().Name);
        }
        [Fact]
        public async void GetProductos_OkResult_PaginationHeaders()
        {
            int shopId = 1;

            var res = await _productoController.GetProducts(shopId);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductoDto>>>(res);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal("{\"TotalItemCount\":2,\"TotalPageCount\":1,\"PageSize\":10,\"CurrentPage\":1}", _httpContext.Response.Headers["X-Pagination"].ToString());
        }

        [Fact]
        public async Task GetProductos_BadRequest_PageSizeExceeded()
        {
            int shopId = 1;
            int pageSize = 101;

            var res = await _productoController.GetProducts(shopId,pageSize);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductoDto>>>(res);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetProductos_NotFound_TiendaDontExists()
        {
            int shopId = 99;

            var res = await _productoController.GetProducts(shopId);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductoDto>>>(res);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetProduct_OkResult()
        {
            int shopId = 1;
            int productId = 1;

            var res = await _productoController.GetProductById(shopId, productId);

            var okResult = Assert.IsType<OkObjectResult>(res);
            var dto = Assert.IsAssignableFrom<ProductoDto>(okResult.Value);
            Assert.Equal("Producto A", dto.Name);
        }
        [Fact]
        public async Task GetProduct_NotFound_TiendaDontExists()
        {
            int shopId = 99;
            int productId = 1;

            var res = await _productoController.GetProductById(shopId, productId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task GetProduct_NotFound_ProductDontExists()
        {
            int shopId = 1;
            int productId = 99;

            var res = await _productoController.GetProductById(shopId, productId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async void CreateProducto_CreatedAtRoute()
        {
            int shopId = 1;
            var productToCreate = new ProductoCreationDto()
            {
                Name = "Nuevo Producto",
                Price = 1.0,
                Description = null
            };

            var res = await _productoController.CreateProduct(shopId, productToCreate);

            var actionResult = Assert.IsType<ActionResult<ProductoDto>>(res);
            var createdAt = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
            var dto = Assert.IsAssignableFrom<ProductoDto>(createdAt.Value);
            Assert.Equal("Nuevo Producto", dto.Name);
        }


        [Fact]
        public async void UpdateProducto_NoContent()
        {
            int shopId = 1;
            int productId = 1;
            var productToUpdate = new ProductoUpdateDto()
            {
                Name = "Producto Actualizado",
                Price = 1.0,
                Description = null
            };

            ActionResult res = await _productoController.UpdateProduct(shopId, productId, productToUpdate);

            Assert.IsType<NoContentResult>(res);
        }


        [Fact]
        public async void DeleteProducto_NoContent()
        {
            int shopId = 1;
            int productId = 1;

            ActionResult res = await _productoController.DeleteProduct(shopId, productId);

            Assert.IsType<NoContentResult>(res);
        }
    }
}
