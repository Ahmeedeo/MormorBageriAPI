namespace MormorBageriAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public Address? Address { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
