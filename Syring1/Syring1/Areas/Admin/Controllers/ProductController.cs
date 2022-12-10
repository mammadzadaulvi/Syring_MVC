using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.FaqQuestion;
using Syring1.Areas.Admin.ViewModels.Product;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index(ProductIndexViewModel model)
        {
            model = new ProductIndexViewModel
            {
                Products = await _appDbContext.Products.Include(p => p.ProductCategory).ToListAsync(),
                ProductCategories = await _appDbContext.ProductCategories.Select(c => new SelectListItem
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
            var model = new ProductCreateViewModel
            {
                Categories = await _appDbContext.ProductCategories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            model.Categories = await _appDbContext.ProductCategories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            if (!ModelState.IsValid) return View(model);
            var category = await _appDbContext.ProductCategories.FindAsync(model.CategoryId);

            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Category not found");
                return View(model);
            }

            bool isExist = await _appDbContext.Products.AnyAsync(p => p.Title.ToLower().Trim() == model.Title.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Title", "This category is already exist");
                return View(model);
            }
            if (!_fileService.IsImage(model.MainPhoto))
            {
                ModelState.AddModelError("MainPhoto", "The image must be img format");
                return View(model);
            }
            if (!_fileService.CheckSize(model.MainPhoto, 300))
            {
                ModelState.AddModelError("MainPhoto", "This image is bigger than 300kb");
                return View(model);
            }



            var product = new Product
            {
                Title = model.Title,
                Price = model.Price,
                ProductCategoryId = model.CategoryId,
                PhotoName = await _fileService.UploadAsync(model.MainPhoto, _webHostEnvironment.WebRootPath),
            };

            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);

            if (product == null) return NotFound();

            var model = new ProductUpdateViewModel
            {
                Title = product.Title,
                Price = product.Price,
                CategoryId = product.ProductCategoryId,
                MainPhotoPath = product.PhotoName,

                Categories = await _appDbContext.ProductCategories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateViewModel model, int id)
        {
            model.Categories = await _appDbContext.ProductCategories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToListAsync();

            if (id != model.Id) return View(model);
            if (id != model.Id) return BadRequest();

            var product = await _appDbContext.Products.FindAsync(id);


            if (product == null) return NotFound();
            bool isExist = await _appDbContext.Products.AnyAsync(p => p.Title.ToLower().Trim() == product.Title.ToLower().Trim() && p.Id != product.Id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "This product is already exist");
                return View(model);
            }


            if (model.MainPhoto != null)
            {

                if (!_fileService.IsImage(model.MainPhoto))
                {
                    ModelState.AddModelError("Photo", "The image must be img format");
                    return View(model);
                }
                if (!_fileService.CheckSize(model.MainPhoto, 300))
                {
                    ModelState.AddModelError("Photo", "The image is bigger than 300kb");
                    return View(model);
                }


                _fileService.Delete(model.MainPhotoPath, _webHostEnvironment.WebRootPath);
                model.MainPhotoPath = await _fileService.UploadAsync(model.MainPhoto, _webHostEnvironment.WebRootPath);
                product.PhotoName = model.MainPhotoPath;
            }

            var category = await _appDbContext.ProductCategories.FindAsync(model.CategoryId);
            if (category == null) return NotFound();
            model.CategoryId = category.Id;

            product.Title = model.Title;
            product.Price = model.Price;
            product.ProductCategoryId = model.CategoryId;

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null) return NotFound();

            _fileService.Delete(product.PhotoName, _webHostEnvironment.WebRootPath);

            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Details
        public async Task<IActionResult> Details(int id)
        {
            var product = await _appDbContext.FaqQuestions.FindAsync(id);
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
