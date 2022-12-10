using static Syring1.Models.FaqCoverPhoto;

namespace Syring1.Areas.Admin.ViewModels.FaqCoverPhoto
{
    public class FaqCoverPhotoDetailsViewModel
    {
        public int Id { get; set; }
        public string PhotoPath { get; set; }
        public PageStatus Status { get; set; }
    }
}
