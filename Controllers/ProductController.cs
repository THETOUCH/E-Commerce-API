using E_Commerce_API.Models;
using E_Commerce_API.Models.DTO;
using E_Commerce_API.Repository;
using E_Commerce_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        
        private readonly IProductService _productService;

        public ProductController(ApplicationContext context, IProductService productService)
        {
            
            _productService = productService;
        }
        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ProductDtoResponse> list = await _productService.GetAll();
            return Ok(list);
        }
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            ProductDtoResponse? product = await _productService.GetById(id);
            return Ok(product);
        }
        [HttpPost("products")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProductDtoRequest dto)
        {
            ProductDtoResponse answer = await _productService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = answer.Id }, answer);
        }
        [HttpPut("products/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDtoRequest dto)
        {
            ProductDtoResponse answer = await _productService.Update(id, dto);
            return Ok(answer);
        }
        [HttpDelete("products/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.Delete(id);
            return NoContent();
        }
    }
}
