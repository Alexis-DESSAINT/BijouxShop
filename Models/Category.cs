using System.Collections.Generic;

namespace BijouxShop.Models
{
    public class Category
    {
        public int Id { get; set; } // PK auto-incrémentée
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; //Slug : chaîne unique, pratique pour /produits/colliers au lieu d’un id numérique.
        public ICollection<Product> Products { get; set; } = new List<Product>(); //Products : la collection inverse (navigation) des produits de cette catégorie.

    }
}
