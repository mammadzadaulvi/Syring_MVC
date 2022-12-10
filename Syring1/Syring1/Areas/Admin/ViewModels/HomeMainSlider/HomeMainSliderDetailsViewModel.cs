namespace Syring1.Areas.Admin.ViewsModels.HomeMainSlider
{
    public class HomeMainSliderDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public ICollection<Models.HomeMainSlider> Photos;
    }
}
