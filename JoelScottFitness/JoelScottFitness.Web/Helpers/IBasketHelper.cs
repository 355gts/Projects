using JoelScottFitness.Common.Models;

namespace JoelScottFitness.Web.Helpers
{
    public interface IBasketHelper
    {
        bool AddItemToBasket(long id, string name, string description, double price);
        BasketItemViewModel DecreaseItemQuantity(long id);
        BasketViewModel GetBasket();
        BasketItemViewModel IncreaseItemQuantity(long id);
        void RemoveItemFromBasket(long id);
        bool AddDiscountCode(DiscountCodeViewModel discountCode);
        void RemoveDiscountCode();
    }
}