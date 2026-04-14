namespace E_Commerce_API.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public int ShoppingCartId { get; set; }
        public Cart ShoppingCart { get; set; }


    }
}
