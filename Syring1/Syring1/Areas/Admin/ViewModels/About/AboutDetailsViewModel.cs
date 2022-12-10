namespace Syring1.Areas.Admin.ViewModels.About
{
    public class AboutDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SignPhotoName { get; set; }
        public ICollection<Models.AboutPhoto> Photos { get; set; }
    }
}
