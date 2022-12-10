namespace Syring1.Areas.Admin.ViewModels.About
{
    public class AboutCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile SignPhoto { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
