using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.About;
using Syring1.Areas.Admin.ViewModels.About.AboutPhoto;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public AboutController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new AboutIndexViewModel
            {
                About = await _appDbContext.Abouts.Include(x => x.Photos).FirstOrDefaultAsync()
            };
            return View(model);

        }


        #region Create 

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var homeIntroSlider = await _appDbContext.Abouts.FirstOrDefaultAsync();
            if (homeIntroSlider != null) return NotFound();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(AboutCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            bool isExist = await _appDbContext.Abouts.AnyAsync(hs => hs.Title.ToLower().Trim() == model.Title.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Choose another Slide name");
                return View(model);
            }

            if (!_fileService.IsImage(model.SignPhoto))
            {
                ModelState.AddModelError("SignPhoto", "Yüklənən fayl image formatında olmalıdır.");
                return View(model);
            }

            if (!_fileService.CheckSize(model.SignPhoto, 300))
            {
                ModelState.AddModelError("SignPhoto", "Şəkilin ölçüsü 300 kb-dan böyükdür");
                return View(model);
            }

            bool hasError = false;
            foreach (var photo in model.Photos)
            {
                if (!_fileService.IsImage(photo))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} yuklediyiniz file sekil formatinda olmalidir");
                    hasError = true;

                }
                else if (!_fileService.CheckSize(photo, 300))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} ci yuklediyiniz sekil 300 kb dan az olmalidir");
                    hasError = true;

                }

            }

            if (hasError) return View(model);

            var homeIntroSlider = new About
            {
                Title = model.Title,
                Description = model.Description,
                SignPhotoName = await _fileService.UploadAsync(model.SignPhoto, _webHostEnvironment.WebRootPath),
            };

            await _appDbContext.Abouts.AddAsync(homeIntroSlider);
            await _appDbContext.SaveChangesAsync();

            int order = 1;
            foreach (var photo in model.Photos)
            {
                var homeIntroSliderPhoto = new AboutPhoto
                {
                    Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                    Order = order,
                    AboutId = homeIntroSlider.Id
                };
                await _appDbContext.AboutPhotos.AddAsync(homeIntroSliderPhoto);
                await _appDbContext.SaveChangesAsync();

                order++;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Update 

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var homeIntroSlider = await _appDbContext.Abouts.Include(hs => hs.Photos).FirstOrDefaultAsync(hs => hs.Id == id);
            if (homeIntroSlider == null) return NotFound();

            var model = new AboutUpdateViewModel
            {
                Title = homeIntroSlider.Title,
                Description = homeIntroSlider.Description,
                SignPhotoName = homeIntroSlider.SignPhotoName,
                AboutPhotos = homeIntroSlider.Photos,
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Update(AboutUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);
            if (id != model.Id) return BadRequest();

            var homeIntroSlider = await _appDbContext.Abouts.Include(hs => hs.Photos).FirstOrDefaultAsync(hs => hs.Id == id);

            model.AboutPhotos = homeIntroSlider.Photos.ToList();
            if (homeIntroSlider == null) return NotFound();



            if (model.SignPhoto != null)
            {

                if (!_fileService.IsImage(model.SignPhoto))
                {
                    ModelState.AddModelError("Photo", "Image formatinda secin");
                    return View(model);
                }
                if (!_fileService.CheckSize(model.SignPhoto, 300))
                {
                    ModelState.AddModelError("Photo", "Sekilin olcusu 300 kb dan boyukdur");
                    return View(model);
                }

                _fileService.Delete(homeIntroSlider.SignPhotoName, _webHostEnvironment.WebRootPath);
                homeIntroSlider.SignPhotoName = await _fileService.UploadAsync(model.SignPhoto, _webHostEnvironment.WebRootPath);
            }

            bool hasError = false;

            if (model.Photos != null)
            {
                foreach (var photo in model.Photos)
                {
                    if (!_fileService.IsImage(photo))
                    {
                        ModelState.AddModelError("Photos", $"{photo.FileName} yuklediyiniz file sekil formatinda olmalidir");
                        hasError = true;
                    }
                    else if (!_fileService.CheckSize(photo, 300))
                    {
                        ModelState.AddModelError("Photos", $"{photo.FileName} ci yuklediyiniz sekil 300 kb dan az olmalidir");
                        hasError = true;
                    }
                }

                if (hasError) { return View(model); }
                var homeIntroSliderPhoto = homeIntroSlider.Photos.OrderByDescending(hs => hs.Order).FirstOrDefault();
                int order = homeIntroSliderPhoto != null ? homeIntroSliderPhoto.Order : 0;
                foreach (var photo in model.Photos)
                {
                    var productPhoto = new AboutPhoto
                    {
                        Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                        Order = ++order,
                        AboutId = homeIntroSlider.Id
                    };
                    await _appDbContext.AboutPhotos.AddAsync(productPhoto);
                    await _appDbContext.SaveChangesAsync();
                }
            }

            homeIntroSlider.Title = model.Title;
            homeIntroSlider.Description = model.Description;
            model.SignPhotoName = homeIntroSlider.SignPhotoName;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete 
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var homeIntroSlider = await _appDbContext.Abouts.Include(hs => hs.Photos).FirstOrDefaultAsync(his => his.Id == id);
            if (homeIntroSlider == null) return NotFound();

            _fileService.Delete(homeIntroSlider.SignPhotoName, _webHostEnvironment.WebRootPath);

            foreach (var photo in homeIntroSlider.Photos)
            {
                _fileService.Delete(photo.Name, _webHostEnvironment.WebRootPath);

            }

            _appDbContext.Abouts.Remove(homeIntroSlider);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Details 

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var homeIntroSlider = await _appDbContext.Abouts.Include(hs => hs.Photos).FirstOrDefaultAsync(his => his.Id == id);

            if (homeIntroSlider == null) return NotFound();

            var model = new AboutDetailsViewModel
            {
                Id = homeIntroSlider.Id,
                Title = homeIntroSlider.Title,
                Description = homeIntroSlider.Description,
                SignPhotoName = homeIntroSlider.SignPhotoName,
                Photos = homeIntroSlider.Photos
            };

            return View(model);
        }
        #endregion


        #region UpdatePhoto 
        
        [HttpGet]
        public async Task<IActionResult> UpdatePhoto(int id)
        {
            var homeIntroSliderPhoto = await _appDbContext.AboutPhotos.FindAsync(id);
            if (homeIntroSliderPhoto == null) return NotFound();

            var model = new AboutPhotoUpdateViewModel
            {
                Order = homeIntroSliderPhoto.Order
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(int id, AboutPhotoUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id != model.Id) return BadRequest();

            var homeIntroSliderPhoto = await _appDbContext.AboutPhotos.FindAsync(model.Id);
            if (homeIntroSliderPhoto == null) return NotFound();

            homeIntroSliderPhoto.Order = model.Order;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("update", "homeintroslider", new { id = homeIntroSliderPhoto.AboutId });

        }
        #endregion

        #region DeletePhoto 

        [HttpGet]
        public async Task<IActionResult> Deletephoto(int id)
        {
            var homeIntroSliderPhoto = await _appDbContext.AboutPhotos.FindAsync(id);
            if (homeIntroSliderPhoto == null) return NotFound();

            _fileService.Delete(homeIntroSliderPhoto.Name, _webHostEnvironment.WebRootPath);

            _appDbContext.AboutPhotos.Remove(homeIntroSliderPhoto);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("update", "homeintroslider", new { id = homeIntroSliderPhoto.AboutId });
        }

        #endregion
    }
}
