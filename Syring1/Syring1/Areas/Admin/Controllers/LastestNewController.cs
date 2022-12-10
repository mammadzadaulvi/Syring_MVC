using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.LastestNew;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;
using System.Reflection.Metadata;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LastestNewController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LastestNewController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = new LastestNewIndexViewModel
            {
                LastestNews = await _appDbContext.LastestNews.ToListAsync()
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
        public async Task<IActionResult> Create(LastestNewCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_fileService.IsImage(model.Photo))
            {
                ModelState.AddModelError("Photo", "Yüklənən fayl image formatında olmalıdır.");
                return View(model);
            }
            if (!_fileService.CheckSize(model.Photo, 300))
            {
                ModelState.AddModelError("Photo", "Şəkilin ölçüsü 300 kb-dan böyükdür");
                return View(model);
            }

            if (model.CreatedDate == null)
            {
                model.CreatedDate = DateTime.Today;
            }

            var expert = new LastestNew
            {
                Title = model.Title,
                Topic = model.Topic,
                CreatedDate = model.CreatedDate.Value,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.LastestNews.AddAsync(expert);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        #endregion

        #region Update 
        

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var lastestNew = await _appDbContext.LastestNews.FindAsync(id);

            if (lastestNew == null) return NotFound();

            var model = new LastestNewUpdateViewModel
            {
                Id = lastestNew.Id,
                Title = lastestNew.Title,
                Topic = lastestNew.Topic,
                CreatedDate = lastestNew.CreatedDate,
                PhotoPath = lastestNew.PhotoPath
            };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Update(LastestNewUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var lastestNew = await _appDbContext.LastestNews.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (lastestNew == null) return NotFound();

            if (model.Photo != null)
            {

                if (!_fileService.IsImage(model.Photo))
                {
                    ModelState.AddModelError("Photo", "Yüklənən fayl image formatında olmalıdır.");
                    return View(model);
                }
                if (!_fileService.CheckSize(model.Photo, 300))
                {
                    ModelState.AddModelError("Photo", "Şəkilin ölçüsü 300 kb-dan böyükdür");
                    return View(model);
                }

                _fileService.Delete(lastestNew.PhotoPath, _webHostEnvironment.WebRootPath);
                lastestNew.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            lastestNew.Title = model.Title;
            lastestNew.Topic = model.Topic;
            lastestNew.CreatedDate = model.CreatedDate.Value;
            model.PhotoPath = lastestNew.PhotoPath;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete 
       

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var lastestNew = await _appDbContext.LastestNews.FindAsync(id);
            if (lastestNew == null) return NotFound();

            _fileService.Delete(lastestNew.PhotoPath, _webHostEnvironment.WebRootPath);

            _appDbContext.LastestNews.Remove(lastestNew);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details 

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var lastestNew = await _appDbContext.LastestNews.FindAsync(id);
            if (lastestNew == null) return NotFound();

            var model = new LastestNewDetailsViewModel
            {
                Id = lastestNew.Id,
                Title = lastestNew.Title,
                Topic = lastestNew.Topic,
                CreatedDate = lastestNew.CreatedDate,
                PhotoPath = lastestNew.PhotoPath
            };
            return View(model);
        }
        #endregion
    }
}
