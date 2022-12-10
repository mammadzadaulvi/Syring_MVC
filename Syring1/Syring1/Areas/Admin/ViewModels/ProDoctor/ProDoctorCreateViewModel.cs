using static Syring1.Models.ProDoctor;

namespace Syring1.Areas.Admin.ViewModels.ProDoctor
{
    public class ProDoctorCreateViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public IFormFile Photo { get; set; }
        public DoctorStatus Status { get; set; }
    }
}
