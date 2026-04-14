using E_Commerce_API.Models.DTO;

namespace E_Commerce_API.Services
{
    public interface IProductService
    {
        Task<ProductDtoResponse> Create(ProductDtoRequest request);
        Task<ProductDtoResponse> Update(int id, ProductDtoRequest request);
        Task<IEnumerable<ProductDtoResponse>> GetAll();
        Task<ProductDtoResponse?> GetById(int id);
        Task Delete(int id);
    }
}
