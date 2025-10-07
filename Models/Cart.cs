using System.Collections.Generic;

namespace BijouxShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string SessionId { get; set; } = string.Empty; // lie le panier à la session/cookie
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }         // FK
        public Cart? Cart { get; set; }

        public int ProductId { get; set; }      // FK obligatoire
        public Product? Product { get; set; }

        public int? VariantId { get; set; }     // FK optionnelle
        public Variant? Variant { get; set; }

        public int Qty { get; set; } = 1;
    }
}