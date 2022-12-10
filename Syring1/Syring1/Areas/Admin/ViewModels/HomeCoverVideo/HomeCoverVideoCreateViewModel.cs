namespace Syring1.Areas.Admin.ViewModels.HomeCoverVideo
{
    public class HomeCoverVideoCreateViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        //public string CoverPhoto { get; set; }
        public IFormFile Photo { get; set; }
    }
}
