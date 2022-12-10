using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.Doctor;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DoctorController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DoctorController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {

            var model = new DoctorIndexViewModel
            {
                Doctors = await _appDbContext.Doctors.ToListAsync()
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

        public async Task<IActionResult> Create(DoctorCreateViewModel model)
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



            if (!_fileService.IsImage(model.CoverPhoto))
            {
                ModelState.AddModelError("Photo", "Yüklənən fayl image formatında olmalıdır.");
                return View(model);
            }
            if (!_fileService.CheckSize(model.CoverPhoto, 300))
            {
                ModelState.AddModelError("Photo", "Şəkilin ölçüsü 300 kb-dan böyükdür");
                return View(model);
            }



            var doctor = new Doctor
            {
                FullName = model.FullName,
                Position = model.Position,
                Qualification = model.Qualification,
                ContactPhone = model.ContactPhone,
                ContactEmail = model.ContactEmail,
                WorkingDay = model.WorkingDay,
                WorkingHour = model.WorkingHour,
                Title = model.Title,
                Description = model.Description,
                SubDescription = model.SubDescription,
                Skill = model.Skill,
                PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath),
                CoverPhotoPath = await _fileService.UploadAsync(model.CoverPhoto, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.Doctors.AddAsync(doctor);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        #endregion

        #region Delete 

        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _appDbContext.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            _fileService.Delete(doctor.PhotoPath, _webHostEnvironment.WebRootPath);
            _fileService.Delete(doctor.CoverPhotoPath, _webHostEnvironment.WebRootPath);

            _appDbContext.Doctors.Remove(doctor);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _appDbContext.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            var model = new DoctorDetailsViewModel
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Position = doctor.Position,
                Qualification = doctor.Qualification,
                ContactPhone = doctor.ContactPhone,
                ContactEmail = doctor.ContactEmail,
                WorkingDay = doctor.WorkingDay,
                WorkingHour = doctor.WorkingHour,
                Title = doctor.Title,
                Description = doctor.Description,
                SubDescription = doctor.SubDescription,
                Skill = doctor.Skill,
                PhotoPath = doctor.PhotoPath,
                CoverPhotoPath = doctor.CoverPhotoPath
            };
            return View(model);
        }
        #endregion

        #region Update
       
        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {
            var doctor = await _appDbContext.Doctors.FindAsync(id);

            if (doctor == null) return NotFound();

            var model = new DoctorUpdateViewModel
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Position = doctor.Position,
                Qualification = doctor.Qualification,
                ContactPhone = doctor.ContactPhone,
                ContactEmail = doctor.ContactEmail,
                WorkingDay = doctor.WorkingDay,
                WorkingHour = doctor.WorkingHour,
                Title = doctor.Title,
                Description = doctor.Description,
                SubDescription = doctor.SubDescription,
                Skill = doctor.Skill,
                PhotoPath = doctor.PhotoPath,
                CoverPhotoPath = doctor.CoverPhotoPath
            };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Update(DoctorUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var doctor = await _appDbContext.Doctors.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (doctor == null) return NotFound();

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

                _fileService.Delete(doctor.PhotoPath, _webHostEnvironment.WebRootPath);
                doctor.PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }


            if (model.CoverPhoto != null)
            {
                if (!_fileService.IsImage(model.CoverPhoto))
                {
                    ModelState.AddModelError("CoverPhoto", "Yüklənən fayl image formatında olmalıdır.");
                    return View(model);
                }
                if (!_fileService.CheckSize(model.CoverPhoto, 300))
                {
                    ModelState.AddModelError("CoverPhoto", "Şəkilin ölçüsü 300 kb-dan böyükdür");
                    return View(model);
                }

                _fileService.Delete(doctor.CoverPhotoPath, _webHostEnvironment.WebRootPath);
                doctor.CoverPhotoPath = await _fileService.UploadAsync(model.CoverPhoto, _webHostEnvironment.WebRootPath);
            }




            doctor.FullName = model.FullName;
            doctor.Position = model.Position;
            doctor.Qualification = model.FullName;
            doctor.ContactPhone = model.Position;
            doctor.ContactEmail = model.FullName;
            doctor.WorkingDay = model.Position;
            doctor.WorkingHour = model.FullName;
            doctor.Title = model.Position;
            doctor.Description = model.FullName;
            doctor.SubDescription = model.Position;
            doctor.Skill = model.Skill;
            doctor.Position = model.Position;
            model.PhotoPath = doctor.PhotoPath;
            model.CoverPhotoPath = doctor.CoverPhotoPath;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
    }
}
