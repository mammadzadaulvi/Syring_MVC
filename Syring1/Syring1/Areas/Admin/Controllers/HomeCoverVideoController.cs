using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.HomeCoverVideo;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeCoverVideoController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeCoverVideoController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {

            var model = new HomeCoverVideoIndexViewModel
            {
                HomeCoverVideo = await _appDbContext.HomeCoverVideo.FirstOrDefaultAsync()
            };
            return View(model);
        }


        #region Create 
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var homeCoverVideo = await _appDbContext.HomeCoverVideo.FirstOrDefaultAsync();
            if (homeCoverVideo != null) return NotFound();
            return View();
        }



        [HttpPost]

        public async Task<IActionResult> Create(HomeCoverVideoCreateViewModel model)
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

            var homeCoverVideo = new HomeCoverVideo
            {
                Id = model.Id,
                Url = model.Url,
                CoverPhoto = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.HomeCoverVideo.AddAsync(homeCoverVideo);
            await _appDbContext.SaveChangesAsync();


            return RedirectToAction("Index");

        }
        #endregion

        #region Delete
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var homeCoverVideo = await _appDbContext.HomeCoverVideo.FindAsync(id);
            if (homeCoverVideo == null) return NotFound();

            _fileService.Delete(homeCoverVideo.CoverPhoto, _webHostEnvironment.WebRootPath);

            _appDbContext.HomeCoverVideo.Remove(homeCoverVideo);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var homeCoverVideo = await _appDbContext.HomeCoverVideo.FindAsync(id);
            if (homeCoverVideo == null) return NotFound();

            var model = new HomeCoverVideoDetailsViewModel
            {
                Id = homeCoverVideo.Id,
                Url = homeCoverVideo.Url,
                CoverPhoto = homeCoverVideo.CoverPhoto
            };
            return View(model);
        }
        #endregion

        #region Update 
        
        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {
            var homeCoverVideo = await _appDbContext.HomeCoverVideo.FindAsync(id);

            if (homeCoverVideo == null) return NotFound();

            var model = new HomeCoverVideoUpdateViewModel
            {
                Id = homeCoverVideo.Id,
                Url = homeCoverVideo.Url,
                CoverPhoto = homeCoverVideo.CoverPhoto
            };
            return View(model);
        }



        [HttpPost]

        public async Task<IActionResult> Update(HomeCoverVideoUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var homeCoverVideo = await _appDbContext.HomeCoverVideo.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (homeCoverVideo == null) return NotFound();

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

                _fileService.Delete(homeCoverVideo.CoverPhoto, _webHostEnvironment.WebRootPath);
                homeCoverVideo.CoverPhoto = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath);
            }

            homeCoverVideo.Url = model.Url;
            model.CoverPhoto = homeCoverVideo.CoverPhoto;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
    }
}
