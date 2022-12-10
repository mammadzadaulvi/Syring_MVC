using Microsoft.AspNetCore.Mvc.Rendering;

namespace Syring1.Areas.Admin.ViewModels.FaqQuestion
{
    public class FaqQuestionCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int FaqCategoryId { get; set; }
        public List<SelectListItem>? FaqCategories { get; set; }
    }
}
