using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.DAL;
using Syring1.Models;
using Syring1.ViewModels.Faq;

namespace Syring1.Controllers
{
    public class FaqController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public FaqController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new FaqIndexViewModel
            {
                FaqCategories = await _appDbContext.FaqCategories.ToListAsync(),
                FaqQuestions = await _appDbContext.FaqQuestions.ToListAsync(),
            };

            return View(model);
        }

        #region Filter

        private async Task<List<FaqQuestion>> GetByCategoryIdAsync(int categoryId)
        {
            return await _appDbContext.FaqQuestions.Where(p => p.FaqCategoryId == categoryId).ToListAsync();
        }


        public async Task<IActionResult> QuestionFilter(int id)
        {
            var faqQuestions = await GetByCategoryIdAsync(id);

            return PartialView("_QuestionPartial", faqQuestions);
        }

        #endregion
    }
}
