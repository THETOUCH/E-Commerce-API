namespace E_Commerce_API.Exceptions
{
    public class NotUniqueEmail : Exception
    {
        public NotUniqueEmail(string message) : base(message) {
        
        }
    }
}
