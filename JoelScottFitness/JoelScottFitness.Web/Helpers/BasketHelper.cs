using JoelScottFitness.Common.Models;
using JoelScottFitness.Web.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoelScottFitness.Web.Helpers
{
    public class BasketHelper : IBasketHelper
    {
        public bool AddItemToBasket(long id, string name, string description, double price)
        {
            var basket = GetBasket();

            if (!basket.Items.ContainsKey(id))
            {
                var basketItem = new BasketItemViewModel()
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Price = price,
                    Quantity = 1,
                };

                // apply the discount code to the new item
                if (basket.DiscountCode != null)
                {
                    basketItem.ItemDiscounted = true;
                    basketItem.DiscountPercent = basket.DiscountCode.PercentDiscount;
                }

                basket.Items.Add(id, basketItem);

                SaveBasket(basket);
                return true;
            }

            return false;
        }

        public void RemoveItemFromBasket(long id)
        {
            var basket = GetBasket();

            if (basket.Items.ContainsKey(id))
            {
                basket.Items.Remove(id);
                SaveBasket(basket);
            }
        }

        public BasketItemViewModel IncreaseItemQuantity(long id)
        {
            var basket = GetBasket();

            if (basket.Items.ContainsKey(id))
            {
                basket.Items[id].Quantity = ++basket.Items[id].Quantity;

                SaveBasket(basket);
            }

            return new BasketItemViewModel() { Id = id, Quantity = basket.Items[id].Quantity };
        }

        public BasketItemViewModel DecreaseItemQuantity(long id)
        {
            var basket = GetBasket();

            // only permit the customer to decrease the quantity to 1
            if (basket.Items.ContainsKey(id) && basket.Items[id].Quantity > 1)
            {
                basket.Items[id].Quantity = --basket.Items[id].Quantity;

                SaveBasket(basket);
            }

            return new BasketItemViewModel() { Id = id, Quantity = basket.Items[id].Quantity };
        }

        public BasketViewModel GetBasket()
        {
            if (HttpContext.Current.Session[SessionKeys.Basket] == null)
            {
                var basket = new BasketViewModel()
                {
                    Items = new Dictionary<long, BasketItemViewModel>()
                };
                SaveBasket(basket);
            }

            return (BasketViewModel)HttpContext.Current.Session[SessionKeys.Basket];
        }

        public bool AddDiscountCode(DiscountCodeViewModel discountCode)
        {
            var basket = GetBasket();

            if (basket.Items != null && basket.Items.Any())
            {
                basket.DiscountCode = discountCode;

                var percentDiscount = basket.DiscountCode.PercentDiscount;
                basket.Items.Values.ToList().ForEach(i =>
                {
                    // apply the discount to all items
                    i.DiscountPercent = percentDiscount;
                    i.ItemDiscounted = true;
                });

                SaveBasket(basket);

                return true;
            }
            return false;
        }

        public void RemoveDiscountCode()
        {
            var basket = GetBasket();

            basket.DiscountCode = null;

            if (basket.Items != null && basket.Items.Any())
            {
                basket.Items.Values.ToList().ForEach(i =>
                {
                    // remove the discount code from all items
                    i.DiscountPercent = 0;
                    i.ItemDiscounted = false;
                });
            }

            SaveBasket(basket);
        }

        private void SaveBasket(BasketViewModel basket)
        {
            HttpContext.Current.Session[SessionKeys.Basket] = basket;
        }
    }
}