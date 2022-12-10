using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.OurVision;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OurVisionController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OurVisionController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = new OurVisionIndexViewModel
            {
                OurVisions = await _appDbContext.OurVisions.ToListAsync()
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
        public async Task<IActionResult> Create(OurVisionCreateViewModel model)
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

            var ourVision = new OurVision
            {
                Title = model.Title,
                Description = model.Description,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.OurVisions.AddAsync(ourVision);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update 


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var ourVision = await _appDbContext.OurVisions.FindAsync(id);

            if (ourVision == null) return NotFound();

            var model = new OurVisionUpdateViewModel
            {
                Id = ourVision.Id,
                Title = ourVision.Title,
                Description = ourVision.Description,
                PhotoPath = ourVision.PhotoPath
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Update(OurVisionUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var ourVision = await _appDbContext.OurVisions.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (ourVision == null) return NotFound();

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

                _fileService.Delete(ourVision.PhotoPath, _webHostEnvironment.WebRootPath);
                ourVision.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            ourVision.Title = model.Title;
            ourVision.Description = model.Description;
            model.PhotoPath = ourVision.PhotoPath;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ourVision = await _appDbContext.OurVisions.FindAsync(id);
            if (ourVision == null) return NotFound();

            _fileService.Delete(ourVision.PhotoPath, _webHostEnvironment.WebRootPath);

            _appDbContext.OurVisions.Remove(ourVision);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var expert = await _appDbContext.OurVisions.FindAsync(id);
            if (expert == null) return NotFound();

            var model = new OurVisionDetailsViewModel
            {
                Id = expert.Id,
                Title = expert.Title,
                Description = expert.Description,
                PhotoPath = expert.PhotoPath
            };
            return View(model);
        }
        #endregion
    }
}
