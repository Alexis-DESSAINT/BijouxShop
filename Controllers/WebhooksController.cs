using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;              // ← important (Events, Event, EventUtility…)
using Stripe.Checkout;     // ← pour Session
using BijouxShop.Data;
using BijouxShop.Models;
namespace BijouxShop.Controllers
{
    [ApiController]
    [Route("webhooks/stripe")]
    public class WebhooksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;
        public WebhooksController(AppDbContext db, IConfiguration cfg)
        {
            _db = db; _cfg = cfg;
        }
        [HttpPost]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new
            StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var sig = Request.Headers["Stripe-Signature"].FirstOrDefault();
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, sig,
                _cfg["Stripe:WebhookSecret"]);
            }
            catch (Exception e)
            {
                return BadRequest($"Webhook Error: {e.Message}");
            }
        if (string.Equals(stripeEvent.Type, "checkout.session.completed", StringComparison.Ordinal))
            {
                var session = stripeEvent.Data.Object as Session;
                var clientRef = session?.ClientReferenceId;
                if (!string.IsNullOrEmpty(clientRef))
                {
                    var cart = await _db.Carts
                    .Include(c => c.Items).ThenInclude(i => i.Product)
                    .Include(c => c.Items).ThenInclude(i => i.Variant)
                    .FirstOrDefaultAsync(c => c.SessionId == clientRef);
                    if (cart != null && cart.Items.Count > 0)
                    {
                        var order = new Order
                        {
                            Status = OrderStatus.PAID,
                            TotalCents = cart.Items.Sum(i =>
                            (i.Product!.PriceCents + (i.Variant?.ExtraCents ?? 0)) * i.Qty),
                            StripeSessionId = session?.Id,
                            ClientReferenceId = clientRef,
                            Items = cart.Items.Select(i => new OrderItem
                            {
                                ProductId = i.ProductId,
                                VariantSku = i.Variant != null ?
                            i.Variant.Sku : null,
                                UnitCents = i.Product!.PriceCents +
                            (i.Variant?.ExtraCents ?? 0),
                                Qty = i.Qty
                            }).ToList()
                        };
                        _db.Orders.Add(order);
                        // Vide le panier (optionnel)
                        _db.CartItems.RemoveRange(cart.Items);
                        await _db.SaveChangesAsync();
                    }
                }
            }
            return Ok();
        }
    }
}
