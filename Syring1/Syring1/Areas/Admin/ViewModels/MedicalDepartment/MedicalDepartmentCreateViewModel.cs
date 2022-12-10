namespace Syring1.Areas.Admin.ViewModels.MedicalDepartment
{
    public class MedicalDepartmentCreateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
    }
}
