namespace E_Commerce_API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public Product()
        {

        }
    }
}
