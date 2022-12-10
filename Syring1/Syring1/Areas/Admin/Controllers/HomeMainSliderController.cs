using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewsModels.HomeMainSlider;
using Syring1.Areas.Admin.ViewsModels.HomeMainSlider.HomeMainSliderPhoto;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeMainSliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public HomeMainSliderController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomeMainSliderIndexViewModel
            {
                HomeMainSlider = await _appDbContext.HomeMainSliders.FirstOrDefaultAsync()
            };
            return View(model);

        }



        #region Create 
       

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var HomeMainSlider = await _appDbContext.HomeMainSliders.FirstOrDefaultAsync();
            if (HomeMainSlider != null) return NotFound();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(HomeMainSliderCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

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

            var HomeMainSlider = new HomeMainSlider
            {
                Title = model.Title,
                Description = model.Description,
                Url = model.Url,
                //Photos = await _fileService.UploadAsync(model.Photos, _webHostEnvironment.WebRootPath),
            };

            await _appDbContext.HomeMainSliders.AddAsync(HomeMainSlider);
            await _appDbContext.SaveChangesAsync();

            int order = 1;
            foreach (var photo in model.Photos)
            {
                var HomeMainSliderPhoto = new HomeMainSliderPhoto
                {
                    Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                    Order = order,
                    HomeMainSliderId = HomeMainSlider.Id
                };
                await _appDbContext.HomeMainSliderPhotos.AddAsync(HomeMainSliderPhoto);
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
            var HomeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos).FirstOrDefaultAsync(hs => hs.Id == id);
            if (HomeMainSlider == null) return NotFound();

            var model = new HomeMainSliderUpdateViewModel
            {
                Title = HomeMainSlider.Title,
                Description = HomeMainSlider.Description,
                Url = HomeMainSlider.Url,
                HomeMainSliderPhotos = HomeMainSlider.HomeMainSliderPhotos,
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Update(HomeMainSliderUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);
            if (id != model.Id) return BadRequest();

            var HomeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos).FirstOrDefaultAsync(hs => hs.Id == id);

            model.HomeMainSliderPhotos = HomeMainSlider.HomeMainSliderPhotos.ToList();
            if (HomeMainSlider == null) return NotFound();


            bool hasError = false;

            if (model.Photos != null)
            {
                foreach (var photo in model.Photos)
                {
                    if (!_fileService.IsImage(photo))
                    {
                        ModelState.AddModelError("Photos", $"{photo.FileName} must be image format");
                        hasError = true;
                    }
                    else if (!_fileService.CheckSize(photo, 300))
                    {
                        ModelState.AddModelError("Photos", $"{photo.FileName} must be lesser than 300 kb");
                        hasError = true;
                    }
                }

                if (hasError) { return View(model); }
                var HomeMainSliderPhoto = HomeMainSlider.HomeMainSliderPhotos.OrderByDescending(hs => hs.Order).FirstOrDefault();
                int order = HomeMainSliderPhoto != null ? HomeMainSliderPhoto.Order : 0;
                foreach (var photo in model.Photos)
                {
                    var productPhoto = new HomeMainSliderPhoto
                    {
                        Name = await _fileService.UploadAsync(photo, _webHostEnvironment.WebRootPath),
                        Order = ++order,
                        HomeMainSliderId = HomeMainSlider.Id
                    };
                    await _appDbContext.HomeMainSliderPhotos.AddAsync(productPhoto);
                    await _appDbContext.SaveChangesAsync();
                }
            }

            HomeMainSlider.Title = model.Title;
            HomeMainSlider.Description = model.Description;
            HomeMainSlider.Url = model.Url;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var HomeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos).FirstOrDefaultAsync(his => his.Id == id);
            if (HomeMainSlider == null) return NotFound();

            //_fileService.Delete(HomeMainSlider.AddPhotoName, _webHostEnvironment.WebRootPath);

            foreach (var photo in HomeMainSlider.HomeMainSliderPhotos)
            {
                _fileService.Delete(photo.Name, _webHostEnvironment.WebRootPath);

            }

            _appDbContext.HomeMainSliders.Remove(HomeMainSlider);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var HomeMainSlider = await _appDbContext.HomeMainSliders.Include(hs => hs.HomeMainSliderPhotos).FirstOrDefaultAsync(his => his.Id == id);

            if (HomeMainSlider == null) return NotFound();

            var model = new HomeMainSliderDetailsViewModel
            {
                Id = HomeMainSlider.Id,
                Title = HomeMainSlider.Title,
                Description = HomeMainSlider.Description,
                Url = HomeMainSlider.Url,
                //Photos = HomeMainSlider.HomeMainSliderPhotos
            };

            return View(model);
        }
        #endregion


        #region UpdatePhoto

        [HttpGet]
        public async Task<IActionResult> UpdatePhoto(int id)
        {
            var HomeMainSliderPhoto = await _appDbContext.HomeMainSliderPhotos.FindAsync(id);
            if (HomeMainSliderPhoto == null) return NotFound();

            var model = new HomeMainSliderPhotoUpdateViewModel
            {
                Order = HomeMainSliderPhoto.Order
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(int id, HomeMainSliderPhotoUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id != model.Id) return BadRequest();

            var HomeMainSliderPhoto = await _appDbContext.HomeMainSliderPhotos.FindAsync(model.Id);
            if (HomeMainSliderPhoto == null) return NotFound();

            HomeMainSliderPhoto.Order = model.Order;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("update", "HomeMainSlider", new { id = HomeMainSliderPhoto.HomeMainSliderId });

        }

        #endregion

        #region DeletePhoto

        [HttpGet]
        public async Task<IActionResult> Deletephoto(int id)
        {
            var HomeMainSliderPhoto = await _appDbContext.HomeMainSliderPhotos.FindAsync(id);
            if (HomeMainSliderPhoto == null) return NotFound();

            _fileService.Delete(HomeMainSliderPhoto.Name, _webHostEnvironment.WebRootPath);

            _appDbContext.HomeMainSliderPhotos.Remove(HomeMainSliderPhoto);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("update", "HomeMainSlider", new { id = HomeMainSliderPhoto.HomeMainSliderId });
        }

        #endregion
    }
}
