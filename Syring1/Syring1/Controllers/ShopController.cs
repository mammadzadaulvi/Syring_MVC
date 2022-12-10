using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.DAL;
using Syring1.Models;
using Syring1.ViewModels.Shop;

namespace Syring1.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public ShopController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new ShopIndexViewModel
            {
                ProductCategories = await _appDbContext.ProductCategories.ToListAsync(),
                Products = await _appDbContext.Products.ToListAsync(),
            };

            return View(model);
        }

        #region Filter

        private async Task<List<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _appDbContext.Products.Where(p => p.ProductCategoryId == categoryId).ToListAsync();
        }


        public async Task<IActionResult> ProductFilter(int id)
        {
            var products = await GetByCategoryIdAsync(id);

            return PartialView("_ProductPartial", products);
        }

        #endregion

    }
}
