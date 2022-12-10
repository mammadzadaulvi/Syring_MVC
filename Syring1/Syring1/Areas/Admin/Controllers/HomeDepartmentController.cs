using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.HomeDepartment;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeDepartmentController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeDepartmentController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomeDepartmentIndexViewModel
            {
                HomeDepartments = await _appDbContext.HomeDepartments.ToListAsync()
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
        public async Task<IActionResult> Create(HomeDepartmentCreateViewModel model)
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

            var HomeDepartment = new HomeDepartment
            {
                Title = model.Title,
                Description = model.Description,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.HomeDepartments.AddAsync(HomeDepartment);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update 
        

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var HomeDepartment = await _appDbContext.HomeDepartments.FindAsync(id);

            if (HomeDepartment == null) return NotFound();

            var model = new HomeDepartmentUpdateViewModel
            {
                Id = HomeDepartment.Id,
                Title = HomeDepartment.Title,
                Description = HomeDepartment.Description,
                PhotoPath = HomeDepartment.PhotoPath
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Update(HomeDepartmentUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var HomeDepartment = await _appDbContext.HomeDepartments.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (HomeDepartment == null) return NotFound();

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

                _fileService.Delete(HomeDepartment.PhotoPath, _webHostEnvironment.WebRootPath);
                HomeDepartment.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            HomeDepartment.Title = model.Title;
            HomeDepartment.Description = model.Description;
            model.PhotoPath = HomeDepartment.PhotoPath;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete 
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var HomeDepartment = await _appDbContext.HomeDepartments.FindAsync(id);
            if (HomeDepartment == null) return NotFound();

            _fileService.Delete(HomeDepartment.PhotoPath, _webHostEnvironment.WebRootPath);

            _appDbContext.HomeDepartments.Remove(HomeDepartment);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details 
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var expert = await _appDbContext.HomeDepartments.FindAsync(id);
            if (expert == null) return NotFound();

            var model = new HomeDepartmentDetailsViewModel
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
