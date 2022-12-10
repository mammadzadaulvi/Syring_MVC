namespace Syring1.ViewModels.Basket
{
    public class BasketIndexViewModel
    {
        public BasketIndexViewModel()
        {
            BasketProducts = new List<BasketProductViewModel>();
        }

        public List<BasketProductViewModel> BasketProducts { get; set; }


        public List<Models.Basket> Baskets { get; set; }


    }
}
