using System.Collections.Generic;
namespace BijouxShop.Models
{

    public enum OrderStatus { PENDING, PAID, SHIPPED, CANCELLED }
    public class Order
    {
        public int Id { get; set; }
        public string? Email { get; set; } // si pas d'auth au début
        public int TotalCents { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.PENDING;
        public string? StripeSessionId { get; set; }
        public string? ClientReferenceId { get; set; } // on y met souvent le SessionId du panier
        public ICollection<OrderItem> Items { get; set; } = new
        List<OrderItem>();
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // FK
        public Order? Order { get; set; }
        public int ProductId { get; set; } // FK
        public Product? Product { get; set; }
        public string? VariantSku { get; set; } // on “fige” le SKU au moment de l’achat
        public int UnitCents { get; set; } // prix unitaire figé (important !)
        public int Qty { get; set; }
    }
}
