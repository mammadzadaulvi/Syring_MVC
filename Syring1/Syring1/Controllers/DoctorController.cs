using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Syring1.ViewModels.Doctor;
using Syring1.DAL;
using Syring1.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Syring1.Models;
using static Syring1.Models.Doctor;

namespace Syring1.Controllers
{
    public class DoctorController : Controller
    {

        private readonly AppDbContext _appDbContext;
        public DoctorController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Pagination And Filter

        private IQueryable<Doctor> FilterDoctors(DoctorIndexViewModel model)
        {
            var finddoctors = FilterByFullname(model.FullName);

            return finddoctors;
        }

        private IQueryable<Doctor> FilterByFullname(string fullname)
        {
            return _appDbContext.Doctors.Where(p => !string.IsNullOrEmpty(fullname) ? p.FullName.Contains(fullname) : true);
        }



        public async Task<IActionResult> Index(DoctorIndexViewModel model)
        {
            var finddoctors = FilterDoctors(model);

            var pageCount = await GetPageCountAsync(model.Take);

            if (model.Page <= 0 || model.Page > pageCount) return NotFound();

            var doctors = await PaginateBlogsAsync(model.Page, model.Take, finddoctors);

            model = new DoctorIndexViewModel
            {
                Doctors = doctors,
                Page = model.Page,
                PageCount = pageCount,
                Take = model.Take
            };
            return View(model);
        }


        private async Task<List<Doctor>> PaginateBlogsAsync(int page, int take, IQueryable<Doctor> doctors)
        {

            return await doctors.OrderByDescending(d => d.Id)
                                      .Skip((page - 1) * take).Take(take)
                                      .ToListAsync();

           
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var doctorsCount = await _appDbContext.Doctors.CountAsync();

            return (int)Math.Ceiling((decimal)doctorsCount / take);
        }

        #endregion

        #region Details

        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _appDbContext.Doctors.FindAsync(id);

            if (doctor == null) return NotFound();

            var model = new DoctorDetailsViewModel
            {
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
                CoverPhotoPath = doctor.CoverPhotoPath,
            };

            return View(model);
        }

        #endregion
    }
}
