namespace MormorBageriAPI.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<RawMaterialSupplier> RawMaterialSuppliers { get; set; } = new List<RawMaterialSupplier>();
    }
}
