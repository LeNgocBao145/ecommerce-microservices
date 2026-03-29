namespace OrderApi.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ClientId { get; set; }
        public int PurchaseQuantity { get; set; }
        public DateTime OrderedDate { get; set; } = DateTime.UtcNow;
    }
}
