namespace Syring1.Areas.Admin.ViewModels.LastestNew
{
    public class LastestNewDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
