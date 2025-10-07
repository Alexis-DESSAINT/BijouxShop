using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BijouxShop.Data;
namespace BijouxShop.Controllers
{
[Route("produits")]
public class ProductsController : Controller
    {
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;
    [HttpGet("")]
    public IActionResult Index()
        {
            var produits = _db.Products
            .Where(p => p.Active)
            .Include(p => p.Images.OrderBy(i => i.Position))
            .OrderByDescending(p => p.Id)
            .ToList();
            return View(produits);
        }
        [HttpGet("{slug}")]
        public IActionResult Details(string slug)
        {
            var p = _db.Products
            .Include(x => x.Images.OrderBy(i => i.Position))
            .Include(x => x.Variants)
            .FirstOrDefault(x => x.Slug == slug);
            if (p == null) return NotFound();
            return View(p);
        }
    }
}
