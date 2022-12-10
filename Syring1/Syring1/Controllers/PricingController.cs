using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.DAL;
using Syring1.Models;
using Syring1.ViewModels.Faq;
using Syring1.ViewModels.Pricing;

namespace Syring1.Controllers
{
    public class PricingController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public PricingController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = new PricingIndexViewModel
            {
                PricingPlans = await _appDbContext.PricingPlans.ToListAsync(),

            };

            return View(model);

        }
    }
}
