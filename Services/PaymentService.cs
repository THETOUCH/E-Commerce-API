using E_Commerce_API.Exceptions;
using E_Commerce_API.Models;
using E_Commerce_API.Repository;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace E_Commerce_API.Services
{
    public class PaymentService
    {
        private readonly ApplicationContext _context;
        public PaymentService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<string> ProcessPayment(int userId)
        {
            Cart? cart = await _context.Carts
                .Include(cart => cart.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);
            if ( cart == null || !cart.Items.Any())
            {
                throw new ValidationException("Cart is empty");
            }
            decimal total = cart.Items.Sum(item => item.Quantity * item.Product.Price);

            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions
            {
                Amount = (long)(total * 100),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                }
            };

            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent intent = await paymentIntentService.CreateAsync(options);

            return intent.ClientSecret;

            
        }
    }
}
