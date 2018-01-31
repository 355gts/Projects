using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Web.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace JoelScottFitness.Web.Helpers
{
    public class BasketHelper : IBasketHelper
    {
        private readonly IJSFitnessService jsfService;

        public BasketHelper(IJSFitnessService jsfService)
        {
            if (jsfService == null)
                throw new ArgumentNullException(nameof(jsfService));

            this.jsfService = jsfService;        
        }

        public async Task AddItemToBasket(long id)
        {
            var basket = GetBasketItems();

            if (!basket.ContainsKey(id))
            {
                var planOption = await jsfService.GetPlanOptionAsync(id);

                if (planOption != null)
                {
                    basket.Add(id, new ItemQuantityViewModel()
                    {
                        Id = id,
                        Price = planOption.Price,
                        Quantity = 1, // 1 is the default quantity
                    });
                }
            }

            HttpContext.Current.Session[SessionKeys.Basket] = basket;
        }

        public void RemoveItemFromBasket(long id)
        {
            var basket = GetBasketItems();

            if (basket.ContainsKey(id))
            {
                basket.Remove(id);
            }

            HttpContext.Current.Session[SessionKeys.Basket] = basket;
        }

        public ItemQuantityViewModel IncreaseItemQuantity(long id)
        {
            var basket = GetBasketItems();

            if (basket.ContainsKey(id))
            {
                basket[id].Quantity = ++basket[id].Quantity;

                HttpContext.Current.Session[SessionKeys.Basket] = basket;
            }

            return new ItemQuantityViewModel() { Id = id, Quantity = basket[id].Quantity };
        }

        public ItemQuantityViewModel DecreaseItemQuantity(long id)
        {
            var basket = GetBasketItems();

            // only permit the customer to decrease the quantity to 1
            if (basket.ContainsKey(id) && basket[id].Quantity > 1)
            {
                basket[id].Quantity = --basket[id].Quantity;

                HttpContext.Current.Session[SessionKeys.Basket] = basket;
            }

            return new ItemQuantityViewModel() { Id = id, Quantity = basket[id].Quantity };
        }

        public IDictionary<long, ItemQuantityViewModel> GetBasketItems()
        {
            if (HttpContext.Current.Session[SessionKeys.Basket] == null)
            {
                HttpContext.Current.Session[SessionKeys.Basket] = new Dictionary<long, ItemQuantityViewModel>();
            }

            return (Dictionary<long, ItemQuantityViewModel>)HttpContext.Current.Session[SessionKeys.Basket];
        }

        public double CalculateTotalCost()
        {
            double total = 0;

            var basket = GetBasketItems();

            foreach (var item in basket)
            {
                total += item.Value.Quantity * item.Value.Price;
            }

            return total;
        }
    }
}