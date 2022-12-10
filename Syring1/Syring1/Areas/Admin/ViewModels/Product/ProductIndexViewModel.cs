using Microsoft.AspNetCore.Mvc.Rendering;

namespace Syring1.Areas.Admin.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public List<Models.Product> Products { get; set; }
        public List<SelectListItem> ProductCategories { get; set; }
    }
}
