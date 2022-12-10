using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.MedicalDepartment;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MedicalDepartmentController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MedicalDepartmentController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = new MedicalDepartmentIndexViewModel
            {
                MedicalDepartments = await _appDbContext.MedicalDepartments.ToListAsync()
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
        public async Task<IActionResult> Create(MedicalDepartmentCreateViewModel model)
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

            var medicalDepartment = new MedicalDepartment
            {
                Title = model.Title,
                Description = model.Description,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.MedicalDepartments.AddAsync(medicalDepartment);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update 

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var medicalDepartment = await _appDbContext.MedicalDepartments.FindAsync(id);

            if (medicalDepartment == null) return NotFound();

            var model = new MedicalDepartmentUpdateViewModel
            {
                Id = medicalDepartment.Id,
                Title = medicalDepartment.Title,
                Description = medicalDepartment.Description,
                PhotoPath = medicalDepartment.PhotoPath
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Update(MedicalDepartmentUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var medicalDepartment = await _appDbContext.MedicalDepartments.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (medicalDepartment == null) return NotFound();

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

                _fileService.Delete(medicalDepartment.PhotoPath, _webHostEnvironment.WebRootPath);
                medicalDepartment.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            medicalDepartment.Title = model.Title;
            medicalDepartment.Description = model.Description;
            model.PhotoPath = medicalDepartment.PhotoPath;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete 
        

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var medicalDepartment = await _appDbContext.MedicalDepartments.FindAsync(id);
            if (medicalDepartment == null) return NotFound();

            _fileService.Delete(medicalDepartment.PhotoPath, _webHostEnvironment.WebRootPath);

            _appDbContext.MedicalDepartments.Remove(medicalDepartment);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var medicalDepartment = await _appDbContext.MedicalDepartments.FindAsync(id);
            if (medicalDepartment == null) return NotFound();

            var model = new MedicalDepartmentDetailsViewModel
            {
                Id = medicalDepartment.Id,
                Title = medicalDepartment.Title,
                Description = medicalDepartment.Description,
                PhotoPath = medicalDepartment.PhotoPath
            };
            return View(model);
        }

        #endregion
    }
}
