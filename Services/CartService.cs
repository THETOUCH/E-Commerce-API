using System.Collections.Generic;
using System.Threading.Tasks;
using E_Commerce_API.Exceptions;
using E_Commerce_API.Models;
using E_Commerce_API.Models.DTO;
using E_Commerce_API.Repository;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationContext _context;
        public CartService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task Add(AddToCartDto dto, int userId)
        {
            if (dto.Quantity <= 0)
            {
                throw new ValidationException("Quantity must be greater than zero.");
            }

            bool productExist = await _context.Products.AnyAsync(product => product.Id == dto.ProductId);
            if (!productExist)
            {
                throw new NotFoundException("Product not found.");
            }

            Cart? cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            CartItem? cartItem = cart.Items.FirstOrDefault(item => item.ProductId == dto.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                });
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task Remove(int productId, int userId)
        {
            Cart? cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found.");
            }

            CartItem? cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem == null)
            {
                throw new NotFoundException("Product not found in cart.");
            }

            cart.Items.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
