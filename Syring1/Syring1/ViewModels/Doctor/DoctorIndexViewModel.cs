namespace Syring1.ViewModels.Doctor
{
    public class DoctorIndexViewModel
    {
        public List<Models.Doctor> Doctors { get; set; }

        public int Page { get; set; } = 1;

        public int Take { get; set; } = 2;

        public int PageCount { get; set; }

        public string? FullName { get; set; }   

    }
}
