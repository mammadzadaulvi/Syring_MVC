namespace Syring1.Models
{
    public class PricingPlan
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public double Price { get; set; }   
        public string Skill { get; set; }   
        public PriceStatus Status { get; set; }


        public enum PriceStatus
        {
            Actice,
            Disable
        }
    }
}
