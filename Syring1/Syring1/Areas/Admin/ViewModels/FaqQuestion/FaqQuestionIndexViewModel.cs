using Microsoft.AspNetCore.Mvc.Rendering;

namespace Syring1.Areas.Admin.ViewModels.FaqQuestion
{
    public class FaqQuestionIndexViewModel
    {
        public List<Models.FaqQuestion> FaqQuestions { get; set; }
        public List<SelectListItem> FaqCategories { get; set; }
    }
}
