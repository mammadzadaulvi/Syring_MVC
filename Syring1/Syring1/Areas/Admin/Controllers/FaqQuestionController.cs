using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.FaqQuestion;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FaqQuestionController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FaqQuestionController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }



        public async Task<IActionResult> Index(FaqQuestionIndexViewModel model)
        {

            model = new FaqQuestionIndexViewModel
            {
                FaqQuestions = await _appDbContext.FaqQuestions.Include(p => p.FaqCategory).ToListAsync(),
                FaqCategories = await _appDbContext.FaqCategories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                })
               .ToListAsync()

            };
            return View(model);
        }


        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new FaqQuestionCreateViewModel
            {
                FaqCategories = await _appDbContext.FaqCategories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Create(FaqQuestionCreateViewModel model)
        {
            model.FaqCategories = await _appDbContext.FaqCategories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            if (!ModelState.IsValid) return View(model);
            var category = await _appDbContext.FaqCategories.FindAsync(model.FaqCategoryId);

            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Category not found");
                return View(model);
            }

            bool isExist = await _appDbContext.FaqQuestions.AnyAsync(p => p.Title.ToLower().Trim() == model.Title.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Title", "This category is already exist");
                return View(model);
            }

            var product = new FaqQuestion
            {
                Title = model.Title,
                Description = model.Description,
                FaqCategoryId = model.FaqCategoryId,
            };

            await _appDbContext.FaqQuestions.AddAsync(product);
            await _appDbContext.SaveChangesAsync();



            /////////////Fikir ver /////////////////
            ///

            return RedirectToAction("Index");

        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _appDbContext.FaqQuestions.FindAsync(id);

            if (product == null) return NotFound();

            var model = new FaqQuestionUpdateViewModel
            {
                Title = product.Title,
                Description = product.Description,
                FaqCategoryId = product.FaqCategoryId,

                FaqCategories = await _appDbContext.FaqCategories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(FaqQuestionUpdateViewModel model, int id)
        {
            model.FaqCategories = await _appDbContext.FaqCategories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            if (id != model.Id) return View(model);
            if (id != model.Id) return BadRequest();

            var product = await _appDbContext.FaqQuestions.FindAsync(id);


            if (product == null) return NotFound();
            bool isExist = await _appDbContext.FaqQuestions.AnyAsync(p => p.Title.ToLower().Trim() == product.Title.ToLower().Trim() && p.Id != product.Id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "This product is already exist");
                return View(model);
            }


            var category = await _appDbContext.FaqCategories.FindAsync(model.FaqCategoryId);
            if (category == null) return NotFound();
            product.FaqCategoryId = category.Id;


            await _appDbContext.SaveChangesAsync();

            product.Title = model.Title;
            product.Description = model.Description;
            product.FaqCategoryId = model.FaqCategoryId;

            return RedirectToAction("Index");

        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _appDbContext.FaqQuestions.FindAsync(id);
            if (product == null) return NotFound();

            _appDbContext.FaqQuestions.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Details
        [HttpGet]

        public async Task<IActionResult> Details(int id)
        {
            var product = await _appDbContext.FaqQuestions.FindAsync(id);
            //var product = await _appDbContext.FaqQuestions.Include(p => p.ProductPhotos).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            var model = new FaqQuestionDetailsViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                FaqCategoryId = product.FaqCategoryId,

                FaqCategories = await _appDbContext.FaqCategories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync()
            };
            return View(model);
        }

        #endregion
    }
}
