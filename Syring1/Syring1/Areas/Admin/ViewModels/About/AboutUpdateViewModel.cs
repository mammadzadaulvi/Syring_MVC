namespace Syring1.Areas.Admin.ViewModels.About
{
    public class AboutUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? SignPhoto { get; set; }
        public string? SignPhotoName { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public ICollection<Models.AboutPhoto>? AboutPhotos { get; set; }
    }
}
