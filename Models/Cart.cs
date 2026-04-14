namespace E_Commerce_API.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
