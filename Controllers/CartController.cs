using E_Commerce_API.Models.DTO;
using E_Commerce_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _cartService.Add(dto, userId);
            return Ok(new
            {
                message = "Added to cart successfully",
                dto.ProductId,
                dto.Quantity
            });
        }
        [HttpDelete]
        [Route("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _cartService.Remove(productId, userId);
            return NoContent();
        }
    }
}
