using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.HomeMedicalCenter;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeMedicalCenterController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeMedicalCenterController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {

            var model = new HomeMedicalCenterIndexViewModel
            {
                HomeMedicalCenter = await _appDbContext.HomeMedicalCenter.FirstOrDefaultAsync()
            };
            return View(model);
        }



        #region Create 
        

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var homeMedicalCenter = await _appDbContext.HomeMedicalCenter.FirstOrDefaultAsync();
            if (homeMedicalCenter != null) return NotFound();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(HomeMedicalCenterCreateViewModel model)
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

            var homeMedicalCenter = new HomeMedicalCenter
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Skill = model.Skill,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.HomeMedicalCenter.AddAsync(homeMedicalCenter);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion

        #region Delete 
        


        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var homeMedicalCenter = await _appDbContext.HomeMedicalCenter.FindAsync(id);
            if (homeMedicalCenter == null) return NotFound();

            _fileService.Delete(homeMedicalCenter.PhotoPath, _webHostEnvironment.WebRootPath);

            _appDbContext.HomeMedicalCenter.Remove(homeMedicalCenter);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var homeMedicalCenter = await _appDbContext.HomeMedicalCenter.FindAsync(id);
            if (homeMedicalCenter == null) return NotFound();

            var model = new HomeMedicalCenterDetailsViewModel
            {
                Id = homeMedicalCenter.Id,
                Title = homeMedicalCenter.Title,
                Description = homeMedicalCenter.Description,
                Skill = homeMedicalCenter.Skill,
                PhotoPath = homeMedicalCenter.PhotoPath
            };
            return View(model);
        }

        #endregion

        #region Update
        


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var homeMedicalCenter = await _appDbContext.HomeMedicalCenter.FindAsync(id);

            if (homeMedicalCenter == null) return NotFound();


            var model = new HomeMedicalCenterUpdateViewModel
            {
                Id = homeMedicalCenter.Id,
                Title = homeMedicalCenter.Title,
                Description = homeMedicalCenter.Description,
                Skill = homeMedicalCenter.Skill,
                PhotoPath = homeMedicalCenter.PhotoPath
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Update(HomeMedicalCenterUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var HomeMedicalCenter = await _appDbContext.HomeMedicalCenter.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (HomeMedicalCenter == null) return NotFound();

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

                _fileService.Delete(HomeMedicalCenter.PhotoPath, _webHostEnvironment.WebRootPath);
                HomeMedicalCenter.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            HomeMedicalCenter.Title = model.Title;
            HomeMedicalCenter.Description = model.Description;
            HomeMedicalCenter.Skill = model.Skill;
            model.PhotoPath = HomeMedicalCenter.PhotoPath;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
    }
}
