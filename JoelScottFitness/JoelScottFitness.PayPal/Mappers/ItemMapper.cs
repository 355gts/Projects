using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using PayPal.Api;

namespace JoelScottFitness.PayPal.Mappers
{
    sealed class ItemMapper : ITypeMapper<SelectedPlanOptionViewModel, Item>
    {
        public Item Map(SelectedPlanOptionViewModel fromObject, Item toObject = null)
        {
            var item = toObject ?? new Item();

            item.currency = "GBP";
            item.description = fromObject.Description;
            item.name = fromObject.Plan.Name;
            item.price = fromObject.Price.ToString();
            item.quantity = fromObject.Quantity.ToString();
            item.sku = fromObject.Id.ToString();

            return item;
        }
    }
}
