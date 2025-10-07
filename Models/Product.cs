using System.Collections.Generic;
namespace BijouxShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int PriceCents { get; set; } // argent en centimes (évite les flottants)
        public int Stock { get; set; } = 0;
        public bool Active { get; set; } = true;
        public int CategoryId { get; set; } // FK vers Category
        public Category? Category { get; set; } // navigation
        public ICollection<Variant> Variants { get; set; } = new
        List<Variant>();
        public ICollection<ProductImage> Images { get; set; } = new
        List<ProductImage>();
    }
}
