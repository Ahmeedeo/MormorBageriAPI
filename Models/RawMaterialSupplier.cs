namespace MormorBageriAPI.Models
{
    public class RawMaterialSupplier
    {
        public int RawMaterialId { get; set; }
        public RawMaterial? RawMaterial { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public decimal Price { get; set; }
    }
}
