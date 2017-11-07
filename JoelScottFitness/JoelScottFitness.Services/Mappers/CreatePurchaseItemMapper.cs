using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreatePurchaseItemMapper : ITypeMapper<SelectedPlanOptionViewModel, PurchasedItem>
    {
        public PurchasedItem Map(SelectedPlanOptionViewModel fromObject, PurchasedItem toObject = null)
        {
            var item = toObject ?? new PurchasedItem();
            
            item.ItemId = fromObject.Id;
            item.Quantity = fromObject.Quantity;

            return item;
        }
    }
}
