namespace RestaurantAPI.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        public List<OrderItem>? OrderItems { get; set; }
    }
}