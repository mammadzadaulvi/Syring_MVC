using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syring1.DAL;
using Syring1.ViewModels.Department;

namespace Syring1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public DepartmentController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DepartmentIndexViewModel
            {
                MedicalDepartments = await _appDbContext.MedicalDepartments.ToListAsync()
            };


            return View(model);
        }
    }
}
