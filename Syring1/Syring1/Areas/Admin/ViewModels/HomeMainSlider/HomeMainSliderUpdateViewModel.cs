namespace Syring1.Areas.Admin.ViewsModels.HomeMainSlider
{
    public class HomeMainSliderUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        //public IFormFile? AddPhoto { get; set; }
        //public string? AddPhotoName { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public ICollection<Models.HomeMainSliderPhoto>? HomeMainSliderPhotos { get; set; }
    }
}
