namespace MormorBageriAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public ICollection<OrderRow> OrderRows { get; set; } = new List<OrderRow>();
    }
}
