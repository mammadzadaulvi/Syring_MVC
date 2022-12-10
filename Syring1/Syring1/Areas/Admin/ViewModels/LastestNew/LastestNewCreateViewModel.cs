namespace Syring1.Areas.Admin.ViewModels.LastestNew
{
    public class LastestNewCreateViewModel
    {
        public string Title { get; set; }
        public string Topic { get; set; }
        public IFormFile Photo { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
