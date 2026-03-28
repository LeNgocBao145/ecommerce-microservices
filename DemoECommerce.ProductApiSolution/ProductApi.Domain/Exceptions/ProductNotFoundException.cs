namespace ProductApi.Domain.Exceptions
{
    public class ProductNotFoundException(string message) : Exception(message);
}
