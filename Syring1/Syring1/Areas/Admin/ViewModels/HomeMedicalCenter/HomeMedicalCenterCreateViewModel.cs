namespace Syring1.Areas.Admin.ViewModels.HomeMedicalCenter
{
    public class HomeMedicalCenterCreateViewModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Skill { get; set; }
        public IFormFile Photo { get; set; }
    }
}
