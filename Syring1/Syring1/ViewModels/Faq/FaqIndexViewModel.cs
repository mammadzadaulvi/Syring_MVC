namespace Syring1.ViewModels.Faq
{
    public class FaqIndexViewModel
    {
        public List<Models.FaqCoverPhoto> FaqCoverPhoto { get; set; }   
        public List<Models.PricingPlan> PricingPlans { get; set; }
        public List<Models.FaqCategory> FaqCategories { get; set; }
        public List<Models.FaqQuestion> FaqQuestions { get; set; }
    }
}
