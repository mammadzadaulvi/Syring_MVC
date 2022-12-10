using static Syring1.Models.PricingPlan;

namespace Syring1.Areas.Admin.ViewModels.PricingPlan
{
    public class PricingPlanDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Skill { get; set; }
        public PriceStatus Status { get; set; }
    }
}
