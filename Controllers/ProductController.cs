using Microsoft.AspNetCore.Mvc;
using MyEcommAPI.Services;
using MyEcommAPI.Models.DTOs;
using MyEcommAPI.Models.Entities;


namespace MyEcommAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            var dtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name  
            }).ToList();
            return Ok(dtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name
            };
            return Ok(dto);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            if (products == null || !products.Any())
                return NotFound();

            var dtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name
            }).ToList();
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)            
                return BadRequest(ModelState);
            
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };

            var createdProduct = await _productService.AddProductAsync(product);

            var resultDto = new ProductDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                CategoryId = createdProduct.CategoryId,
                CategoryName = createdProduct.Category?.Name
            };
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Stock = dto.Stock;
            existingProduct.CategoryId = dto.CategoryId;

            var updatedProduct = await _productService.UpdateProductAsync(existingProduct);

            var resultDto = new ProductDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                CategoryId = updatedProduct.CategoryId,
                CategoryName = updatedProduct.Category?.Name
            };
            return Ok(resultDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)           
                return NotFound();
            
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        
    }
}