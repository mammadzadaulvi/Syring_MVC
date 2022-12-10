namespace Syring1.Areas.Admin.ViewModels.Doctor
{
    public class DoctorUpdateViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Qualification { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string WorkingDay { get; set; }
        public string WorkingHour { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubDescription { get; set; }
        public string Skill { get; set; }


        public string? CoverPhotoPath { get; set; }
        public IFormFile? CoverPhoto { get; set; }

        
        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
