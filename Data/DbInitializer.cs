using System.Linq;
using System.Collections.Generic;
using BijouxShop.Models;

namespace BijouxShop.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext db)
        {
            db.Database.EnsureCreated();
            if (db.Categories.Any()) return;

            var catRings = new Category { Name = "Bagues", Slug = "bagues" };
            var catNeck  = new Category { Name = "Colliers", Slug = "colliers" };
            db.Categories.AddRange(catRings, catNeck);
            db.SaveChanges();

            var p1 = new Product
            {
                Name = "Bague Étoile Polaris",
                Slug = "bague-etoile-polaris",
                Description = "Bague fine inspirée du ciel nocturne.",
                PriceCents = 8900,
                Stock = 20,
                CategoryId = catRings.Id,
                Images = new List<ProductImage>
                {
                    new ProductImage { Url = "/img/polaris.jpg", Alt = "Bague Polaris" }
                },
                Variants = new List<Variant>
                {
                    new Variant { Name = "Or 18k / 52", Sku = "POL-OR18-52", Stock = 5 },
                    new Variant { Name = "Argent / 54", Sku = "POL-ARG-54", Stock = 8 }
                }
            };

            db.Products.Add(p1);
            db.SaveChanges();
        }
    }
}
