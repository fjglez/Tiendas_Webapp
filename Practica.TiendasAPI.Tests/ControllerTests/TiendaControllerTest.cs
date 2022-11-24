using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
using System.Text;
using System.Threading.Tasks;

namespace Practica.TiendasAPI.Tests.ControllerTests
{
    public class TiendaControllerTest : IDisposable
    {
        private TiendaController _tiendaController;
        private HttpContext _httpContext;
        public void Dispose()
        {
            
        }

        public TiendaControllerTest()
        {
            var MockLogger = new Mock<ILogger<TiendaController>>();
            var MockRepository = new Mock<ITiendaRepository>();

            // Mock de Mapper
            //var MockMapper = new Mock<IMapper>();
            //MockMapper.Setup(m => m.Map<Tienda, TiendaDto>(It.IsAny<Tienda>())).Returns(new TiendaDto());
            
            var mapperConf = new MapperConfiguration(cfg => cfg.AddProfile<TiendaProfile>());
            var mapper = new Mapper(mapperConf);

            // Mock de HttpContext
            //var httpContextMock = new Mock<HttpContext>();
            //var response = new Mock<HttpResponse>();
            //var header = new Mock<IHeaderDictionary>();
            //response.SetupGet(x => x.Headers).Returns(header.Object);
            //httpContextMock.SetupGet(x => x.Response).Returns(response.Object);

            _httpContext = new DefaultHttpContext();

            MockRepository.Setup(m => m.GetTiendaAsync(1,It.IsAny<bool>()))
                .ReturnsAsync(new Tienda() { Name="Tienda A" });
            MockRepository.Setup(m => m.GetTiendaAsync(99, false))
                .ReturnsAsync((Tienda)null);
            MockRepository.Setup(m => m.GetTiendasAsync(null, null, 10, 1,true))
                .ReturnsAsync((new List<Tienda>()
                {
                    new Tienda(){Name="Tienda A",Id=1},
                    new Tienda(){Name="Tienda B",Id=2}
                }, new PaginationMetadata(3,10,1)));
            _tiendaController = new TiendaController(MockLogger.Object, MockRepository.Object, mapper);

            _tiendaController.ControllerContext = new ControllerContext()
            {
                HttpContext = _httpContext,
            };
        }

        [Fact]
        public async Task GetTiendas_OkResult_CountResults()
        {
            var res = await _tiendaController.GetShops(null, null);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<TiendaDto>>>(res);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<TiendaDto>>(okResult.Value);
            Assert.Equal(2,dtos.Count());
            Assert.Equal("Tienda A", dtos.First().Name);
        }

        [Fact]
        public async Task GetTiendas_OkResult_PaginationHeaders()
        {
            var res = await _tiendaController.GetShops(null, null);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<TiendaDto>>>(res);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal("{\"TotalItemCount\":3,\"TotalPageCount\":1,\"PageSize\":10,\"CurrentPage\":1}", _httpContext.Response.Headers["X-Pagination"].ToString());
        }

        [Fact]
        public async Task GetTiendas_BadRequest_PageSizeExceeded()
        {
            int pageSize = 101;
            var res = await _tiendaController.GetShops(null, null, pageSize);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<TiendaDto>>>(res);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetTienda_OkResult_TiendaSinProductosObject()
        {
            int shopId = 1;

            IActionResult res = await _tiendaController.GetShopById(shopId, false);

            var okResult = Assert.IsType<OkObjectResult>(res);
            var dto = Assert.IsAssignableFrom<TiendaSinProductosDto>(okResult.Value);
            Assert.Equal("Tienda A", dto.Name);
        }

        [Fact]
        public async Task GetTienda_OkResult_TiendaDtoObject()
        {
            int shopId = 1;

            IActionResult res = await _tiendaController.GetShopById(shopId, true);

            var okResult = Assert.IsType<OkObjectResult>(res);
            var dto = Assert.IsAssignableFrom<TiendaDto>(okResult.Value);
            Assert.Equal("Tienda A", dto.Name);
        }

        [Fact]
        public async Task GetTienda_NotFoundResult()
        {
            int shopId = 99;

            IActionResult res = await _tiendaController.GetShopById(shopId, false);

            Assert.IsType<NotFoundResult>(res);
        }
    }
}
