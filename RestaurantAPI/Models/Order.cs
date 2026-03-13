namespace RestaurantAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public List<OrderItem>? OrderItems { get; set; }
    }
}