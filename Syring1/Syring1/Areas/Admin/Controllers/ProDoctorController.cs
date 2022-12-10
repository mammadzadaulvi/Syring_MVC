using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.ProDoctor;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProDoctorController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProDoctorController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {

            var model = new ProDoctorIndexViewModel
            {
                ProDoctors = await _appDbContext.ProDoctors.ToListAsync()
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
        public async Task<IActionResult> Create(ProDoctorCreateViewModel model)
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

            var proDoctor = new ProDoctor
            {
                FullName = model.FullName,
                Icon = model.Icon,
                Status = model.Status,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.ProDoctors.AddAsync(proDoctor);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        #endregion

        #region Delete 
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var proDoctor = await _appDbContext.ProDoctors.FindAsync(id);
            if (proDoctor == null) return NotFound();

            _fileService.Delete(proDoctor.PhotoPath, _webHostEnvironment.WebRootPath);

            _appDbContext.ProDoctors.Remove(proDoctor);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details 

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var proDoctor = await _appDbContext.ProDoctors.FindAsync(id);
            if (proDoctor == null) return NotFound();

            var model = new ProDoctorDetailsViewModel
            {
                Id = proDoctor.Id,
                FullName = proDoctor.FullName,
                Icon = proDoctor.Icon,
                Status = proDoctor.Status,
                PhotoPath = proDoctor.PhotoPath
            };
            return View(model);
        }
        #endregion

        #region Update

        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {
            var proDoctor = await _appDbContext.ProDoctors.FindAsync(id);

            if (proDoctor == null) return NotFound();

            var model = new ProDoctorUpdateViewModel
            {
                Id = proDoctor.Id,
                FullName = proDoctor.FullName,
                Icon = proDoctor.Icon,
                Status = proDoctor.Status,
                PhotoPath = proDoctor.PhotoPath
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Update(ProDoctorUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var proDoctor = await _appDbContext.ProDoctors.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (proDoctor == null) return NotFound();

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

                _fileService.Delete(proDoctor.PhotoPath, _webHostEnvironment.WebRootPath);
                proDoctor.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            proDoctor.FullName = model.FullName;
            proDoctor.Icon = model.Icon;
            proDoctor.Status = model.Status;
            model.PhotoPath = proDoctor.PhotoPath;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
    }
}
