namespace E_Commerce_API.Models.DTO
{
    public class ProductDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductDtoResponse()
        {
        }
    }
}
