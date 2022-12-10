namespace Syring1.Models
{
    public class About
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SignPhotoName { get; set; }
        public ICollection<AboutPhoto> Photos { get; set; }
    }
}
