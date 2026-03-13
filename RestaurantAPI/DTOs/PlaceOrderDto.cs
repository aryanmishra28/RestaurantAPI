namespace RestaurantAPI.DTOs
{
    public class PlaceOrderDto
    {
        public int CustomerId { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
