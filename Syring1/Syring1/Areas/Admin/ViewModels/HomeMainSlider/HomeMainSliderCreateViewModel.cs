namespace Syring1.Areas.Admin.ViewsModels.HomeMainSlider
{
    public class HomeMainSliderCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; } 
        public List<IFormFile> Photos { get; set; }
    }
}
