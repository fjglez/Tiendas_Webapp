using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Practica.TiendasAPI.Models;
using Practica.TiendasAPI.Services;

namespace Practica.TiendasAPI.Controllers
{
    [ApiController]
    [Route("api/shops")]
    public class TiendaController : Controller
    {
        private const int MaxPageSize = 100;
        private readonly ILogger<TiendaController> _logger;
        private readonly ITiendaRepository _shopRepository;
        private readonly IMapper _mapper;
        public TiendaController(ILogger<TiendaController> logger,
            ITiendaRepository shopRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _shopRepository = shopRepository ?? throw new ArgumentNullException(nameof(shopRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Lista todas las tiendas con sus productos.
        /// </summary>
        /// <param name="filterBy">Filtro por nombre de la tienda</param>
        /// <param name="query">Consulta a buscar en el nombre de la tienda</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pagination">Incluir o no paginación en el resultado</param>
        /// <returns>Lista de tiendas en formato JSON.</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TiendaDto>>> GetShops(string? filterBy, 
            string? query, int pageSize=10, int pageNumber=1, bool pagination = true)
        {
            if(pageSize > MaxPageSize)
            {
                _logger.LogWarning($"Se ha intentado introducir un tamaño de página demasiado grande: {pageSize}.");
                return BadRequest("El número máximo de tamaño de página es de " + MaxPageSize + ".");
            }
            var (shops, metadata) = await _shopRepository.GetTiendasAsync(filterBy, query, pageSize, pageNumber,pagination);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            return Ok(_mapper.Map<IEnumerable<TiendaDto>>(shops));
        }
        /// <summary>
        /// Obtiene una tienda.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="includeProducts">Indica si el resultado debe incluir o no los productos de la tienda.</param>
        /// <returns>Datos de la tienda en formato JSON.</returns>
        [HttpGet("{shopId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetShopById(int shopId, bool includeProducts=true)
        {
            var shop = await _shopRepository.GetTiendaAsync(shopId, includeProducts);
            if(shop == null)
            {
                return NotFound();
            }
            if (includeProducts)
            {
                return Ok(_mapper.Map<TiendaDto>(shop));
            }
            else
            {
                return Ok(_mapper.Map<TiendaSinProductosDto>(shop));
            }
        }
    }
}
