namespace Syring1.Models
{
    public class FaqCoverPhoto
    {
        public int Id { get; set; }
        public string CoverPhoto { get; set; }
        public PageStatus Status { get; set; }

        public enum PageStatus
        {
            Faq,
            Pricing
        }
    }
}
