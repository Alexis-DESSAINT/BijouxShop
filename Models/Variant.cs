namespace BijouxShop.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // ex: "Or 18k / 52"
        public string Sku { get; set; } = string.Empty; // code unique (stock & logistique)
        public int ExtraCents { get; set; } = 0; // supplément sur le prix du Product, prix = Product.PriceCents + Variant.ExtraCents
        public int ProductId { get; set; } // FK
        public Product? Product { get; set; }
        public int Stock { get; set; } = 0;
    }
}
