using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using BijouxShop.Data;
namespace BijouxShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _db;
        private string AppUrl => $"{Request.Scheme}://{Request.Host}";
        public CheckoutController(AppDbContext db) { _db = db; }
        private string GetSessionId()
        {
            const string key = "SessionId";
            var sid = HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(sid))
            {
                sid =
            Guid.NewGuid().ToString(); HttpContext.Session.SetString(key, sid);
            }
            return sid;
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Start()
        {
            var sid = GetSessionId();
            var cart = _db.Carts
            .Include(c => c.Items).ThenInclude(i => i.Product)
            .Include(c => c.Items).ThenInclude(i => i.Variant)
            .FirstOrDefault(c => c.SessionId == sid);
            if (cart == null || cart.Items.Count == 0) return Redirect("/panier");
            var lineItems = cart.Items.Select(i => new SessionLineItemOptions
            {
                Quantity = i.Qty,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "eur",
                    UnitAmount = i.Product!.PriceCents +
            (i.Variant?.ExtraCents ?? 0),
                    ProductData = new
            SessionLineItemPriceDataProductDataOptions
                    { Name = i.Product!.Name }
                }
            }).ToList();
            var options = new SessionCreateOptions
{
Mode = "payment",
ClientReferenceId = sid, // on s'en servira dans le webhook
LineItems = lineItems,
SuccessUrl = AppUrl + "/success",
CancelUrl = AppUrl + "/panier"
};
            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}
