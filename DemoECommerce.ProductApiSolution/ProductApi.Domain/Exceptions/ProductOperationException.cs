namespace ProductApi.Domain.Exceptions
{
    public class ProductOperationException(string message) : Exception(message);
}
