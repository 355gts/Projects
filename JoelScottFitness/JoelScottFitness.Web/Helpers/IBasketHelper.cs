using JoelScottFitness.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoelScottFitness.Web.Helpers
{
    public interface IBasketHelper
    {
        Task AddItemToBasket(long id);
        double CalculateTotalCost();
        ItemQuantityViewModel DecreaseItemQuantity(long id);
        IDictionary<long, ItemQuantityViewModel> GetBasketItems();
        ItemQuantityViewModel IncreaseItemQuantity(long id);
        void RemoveItemFromBasket(long id);
    }
}