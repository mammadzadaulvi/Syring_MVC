using Microsoft.AspNetCore.Mvc.Rendering;

namespace Syring1.Areas.Admin.ViewModels.Product
{
    public class ProductCreateViewModel
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public IFormFile MainPhoto { get; set; }
    }
}
