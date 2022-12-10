using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.DAL;
using Syring1.ViewModels.Home;

namespace Syring1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                HomeMainSlider = await _appDbContext.HomeMainSliders.Include(his => his.HomeMainSliderPhotos.OrderBy(his => his.Order)).FirstOrDefaultAsync(),
                OurVisions = await _appDbContext.OurVisions.ToListAsync(),
                HomeDepartments = await _appDbContext.HomeDepartments.ToListAsync(),
                LastestNews = await _appDbContext.LastestNews.ToListAsync(),
                ProDoctors = await _appDbContext.ProDoctors.ToListAsync(),
                HomeMedicalCenter = await _appDbContext.HomeMedicalCenter.FirstOrDefaultAsync(),
                HomeCoverVideo = await _appDbContext.HomeCoverVideo.FirstOrDefaultAsync(),
                About = await _appDbContext.Abouts.Include(abt => abt.Photos.OrderBy(abt => abt.Order)).FirstOrDefaultAsync()
            };


            return View(model);
        }
    }
}
