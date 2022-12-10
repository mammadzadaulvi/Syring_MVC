using Microsoft.AspNetCore.Mvc.Rendering;

namespace Syring1.Areas.Admin.ViewModels.Product
{
    public class ProductUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public string? MainPhotoPath { get; set; }

        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
