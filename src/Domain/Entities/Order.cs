namespace OrderSystem.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Status { get; set; } = "created";
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
