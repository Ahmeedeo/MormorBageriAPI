namespace MormorBageriAPI.Models
{
    public class OrderDto
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderProductDto>? Products { get; set; }
    }

    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
