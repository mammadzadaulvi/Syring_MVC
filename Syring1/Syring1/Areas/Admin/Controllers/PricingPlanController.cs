using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.Areas.Admin.ViewModels.PricingPlan;
using Syring1.DAL;
using Syring1.Helpers;
using Syring1.Models;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PricingPlanController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PricingPlanController(AppDbContext appDbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {

            var model = new PricingPlanIndexViewModel
            {
                PricingPlans = await _appDbContext.PricingPlans.ToListAsync()
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

        public async Task<IActionResult> Create(PricingPlanCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var pricingPlan = new PricingPlan
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                Skill = model.Skill,
                Status = model.Status,
                //PhotoPath = await _fileService.UploadAsync(model.Photo, _webHostEnvironment.WebRootPath)
            };

            await _appDbContext.PricingPlans.AddAsync(pricingPlan);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        #endregion

        #region Delete
       

        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {
            var pricingPlan = await _appDbContext.PricingPlans.FindAsync(id);
            if (pricingPlan == null) return NotFound();

            //_fileService.Delete(expert.PhotoPath, _webHostEnvironment.WebRootPath);

            _appDbContext.PricingPlans.Remove(pricingPlan);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var pricingPlan = await _appDbContext.PricingPlans.FindAsync(id);
            if (pricingPlan == null) return NotFound();

            var model = new PricingPlanDetailsViewModel
            {
                Id = pricingPlan.Id,
                Title = pricingPlan.Title,
                Description = pricingPlan.Description,
                Price = pricingPlan.Price,
                Skill = pricingPlan.Skill,
                Status = pricingPlan.Status,
            };
            return View(model);
        }
        #endregion

        #region Update

        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {
            var expert = await _appDbContext.PricingPlans.FindAsync(id);

            if (expert == null) return NotFound();


            var model = new PricingPlanUpdateViewModel
            {
                Id = expert.Id,
                Title = expert.Title,
                Description = expert.Description,
                Price = expert.Price,
                Skill = expert.Skill,
                Status = expert.Status,
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Update(PricingPlanUpdateViewModel model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            var pricingPlan = await _appDbContext.PricingPlans.FindAsync(id);

            if (id != model.Id) return BadRequest();

            if (pricingPlan == null) return NotFound();


            pricingPlan.Title = model.Title;
            pricingPlan.Description = model.Description;
            pricingPlan.Price = model.Price;
            pricingPlan.Skill = model.Skill;
            pricingPlan.Status = model.Status;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
    }
}
