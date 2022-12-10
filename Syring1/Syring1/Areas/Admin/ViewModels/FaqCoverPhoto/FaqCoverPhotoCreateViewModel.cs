using static Syring1.Models.FaqCoverPhoto;

namespace Syring1.Areas.Admin.ViewModels.FaqCoverPhoto
{
    public class FaqCoverPhotoCreateViewModel
    {
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
        public PageStatus Status { get; set; }
    }
}
