namespace BijouxShop.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Alt { get; set; }
        public int ProductId { get; set; } // FK
        public Product? Product { get; set; }
        public int Position { get; set; } = 0;  // pour ordonner
    }
}