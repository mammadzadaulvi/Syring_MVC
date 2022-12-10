namespace Syring1.Models
{
    public class FaqQuestion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int FaqCategoryId { get; set; }

        public FaqCategory FaqCategory { get; set; }
    }
}
