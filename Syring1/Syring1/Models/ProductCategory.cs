namespace Syring1.Models
{
    public class ProductCategory
    {
        public int Id{ get; set; }
        public string Title { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
