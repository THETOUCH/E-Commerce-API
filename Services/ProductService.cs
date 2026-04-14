using E_Commerce_API.Exceptions;
using E_Commerce_API.Models;
using E_Commerce_API.Models.DTO;
using E_Commerce_API.Repository;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationContext _context;

        public ProductService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ProductDtoResponse> Create(ProductDtoRequest request)
        {
            Product product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return new ProductDtoResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }
        public async Task<ProductDtoResponse> Update(int id, ProductDtoRequest request)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(product => product.Id == id);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            await _context.SaveChangesAsync();
            return new ProductDtoResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }
        public async Task Delete(int id)
        {
            Product? product = _context.Products.FirstOrDefault(product => product.Id == id);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProductDtoResponse>> GetAll()
        {
            return await _context.Products
                .Select(product => new ProductDtoResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                }).ToListAsync();
        }
        public async Task<ProductDtoResponse> GetById(int id)
        {
            Product? product = _context.Products.FirstOrDefault(product => product.Id == id);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }
            return new ProductDtoResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }

    }
}
