namespace Syring1.Areas.Admin.ViewModels.HomeDepartment
{
    public class HomeDepartmentUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
