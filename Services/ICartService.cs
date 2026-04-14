using E_Commerce_API.Models.DTO;

namespace E_Commerce_API.Services
{
    public interface ICartService
    {
        Task Add(AddToCartDto dto, int userId);
        Task Remove(int productId, int userId);
    }
}
