using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.FaqCategory;
using Syring1.DAL;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FaqCategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FaqCategoryController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var model = new FaqCategoryIndexViewModel
            {
                FaqCategories = await _appDbContext.FaqCategories.ToListAsync()
            };
            return View(model);
        }

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FaqCategoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            bool isExist = await _appDbContext.FaqCategories
                            .AnyAsync(c => c.Title.ToLower().Trim() == model.Title.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "This title is already exist");
                return View(model);
            }

            var faqCategory = new FaqCategory
            {
                Title = model.Title,
                Description = model.Description
            };

            await _appDbContext.FaqCategories.AddAsync(faqCategory);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _appDbContext.FaqCategories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, FaqCategoryUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id != model.Id) return BadRequest();

            var dbFaqCategory = await _appDbContext.FaqCategories.FindAsync(id);
            if (dbFaqCategory == null) return NotFound();

            bool isExist = await _appDbContext.FaqCategories.AnyAsync(ct => ct.Title.ToLower().Trim() == model.Title.ToLower().Trim() && ct.Id != model.Id);
            if (isExist)
            {
                ModelState.AddModelError("Title", "This component is already exist");
                return View(model);
            }
            dbFaqCategory.Title = model.Title;
            dbFaqCategory.Description = model.Description;


            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var faqCategory = await _appDbContext.FaqCategories.FindAsync(id);
            if (faqCategory == null) return NotFound();

            _appDbContext.FaqCategories.Remove(faqCategory);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var faqCategory = await _appDbContext.FaqCategories.FindAsync(id);
            if (faqCategory == null) return NotFound();
            return View(faqCategory);
        }

        #endregion
    }
}
