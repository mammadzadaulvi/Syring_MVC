namespace Syring1.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string PhotoName { get; set; }
        public double Price { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public List<BasketProduct> BasketProducts { get; set; }
    }
}
