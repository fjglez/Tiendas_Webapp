using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Practica.TiendasAPI.Entities;
using Practica.TiendasAPI.Models;
using Practica.TiendasAPI.Services;
using System.Text.Json;

namespace Practica.TiendasAPI.Controllers
{
    [ApiController]
    [Route("api/shops/{shopId}/products")]
    public class ProductoController : Controller
    {
        private const int MaxPageSize = 100;
        private readonly ILogger<ProductoController> _logger;
        private readonly ITiendaRepository _shopRepository;
        private readonly IMapper _mapper;
        public ProductoController(ILogger<ProductoController> logger,
            ITiendaRepository shopRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _shopRepository = shopRepository ?? throw new ArgumentNullException(nameof(shopRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Lista todos los productos de una tienda.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pagination">Incluir o no paginación en el resultado</param>
        /// <returns>Lista de productos en JSON</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProducts(int shopId, 
            int pageSize = 10, int pageNumber = 1, bool pagination = true)
        {
            if (!await _shopRepository.TiendaExistsAsync(shopId))
            {
                return NotFound();
            }
            if (pageSize > MaxPageSize)
            {
                _logger.LogWarning($"Se ha intentado introducir un tamaño de página demasiado grande: {pageSize}.");
                return BadRequest("El número máximo de tamaño de página es de " + MaxPageSize + ".");
            }
            var (products, metadata) = await _shopRepository.GetProductosAsync(shopId, pageSize, pageNumber, pagination);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(_mapper.Map<IEnumerable<ProductoDto>>(products));
        }
        /// <summary>
        /// Obtiene un producto de una tienda.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="productId"></param>
        /// <returns>Información de un producto en JSON</returns>
        [HttpGet("{productId}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> GetProductById(int shopId, int productId)
        {
            if (!await _shopRepository.TiendaExistsAsync(shopId))
            {
                return NotFound();
            }
            var product = await _shopRepository.GetProductoAsync(shopId, productId);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductoDto>(product));
        }
        /// <summary>
        /// Crea un producto en la tienda específicada.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="product">Nuevo producto en formato JSON</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost()]
        public async Task<ActionResult<ProductoDto>> CreateProduct(int shopId, ProductoCreationDto product)
        {
            if (!await _shopRepository.TiendaExistsAsync(shopId))
            {
                return NotFound();
            }
            var newProduct = _mapper.Map<Producto>(product);
            await _shopRepository.AddProductoAsync(shopId, newProduct);
            await _shopRepository.SaveChangesAsync();

            var createdProduct = _mapper.Map<ProductoDto>(newProduct);
            return CreatedAtRoute("GetProduct", new
            {
                shopId = shopId,
                productId = createdProduct.Id
            }, createdProduct);
        }
        /// <summary>
        /// Modifica un producto en la tienda especificada.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="productId"></param>
        /// <param name="product">Nuevo producto en formato JSON</param>
        /// <returns></returns>
        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateProduct(int shopId, int productId, ProductoUpdateDto product)
        {
            if (!await _shopRepository.TiendaExistsAsync(shopId))
            {
                return NotFound();
            }
            var productToModify = await _shopRepository.GetProductoAsync(shopId, productId);
            if (productToModify == null)
            {
                return NotFound();
            }
            _mapper.Map(product, productToModify);
            await _shopRepository.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Modifica parcialmente un producto en la tienda especificada.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="productId"></param>
        /// <param name="patch">Cambios a realizar en formato JsonPatchDocument</param>
        /// <returns></returns>
        [HttpPatch("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PatchProduct(int shopId, int productId, 
            JsonPatchDocument<ProductoUpdateDto> patch)
        {
            if (!await _shopRepository.TiendaExistsAsync(shopId))
            {
                return NotFound();
            }
            var productEntity = await _shopRepository.GetProductoAsync(shopId, productId);
            if (productEntity == null)
            {
                return NotFound();
            }
            var productToPatch = _mapper.Map<ProductoUpdateDto>(productEntity);
            patch.ApplyTo(productToPatch,ModelState);
            if(!ModelState.IsValid || !TryValidateModel(productToPatch))
            {
                return BadRequest();
            }

            _mapper.Map(productToPatch, productEntity);
            await _shopRepository.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Elimina un producto.
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult> DeleteProduct(int shopId, int productId)
        {
            if (!await _shopRepository.TiendaExistsAsync(shopId))
            {
                return NotFound();
            }
            var productEntity = await _shopRepository.GetProductoAsync(shopId, productId);
            if (productEntity == null)
            {
                return NotFound();
            }

            _shopRepository.RemoveProducto(productEntity);
            await _shopRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
