using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;

namespace JoelScottFitness.Services.Mappers
{
    sealed class ItemMapper : ITypeMapper<Item, ItemViewModel>, ITypeMapper<ItemViewModel, Item>
    {
        public ItemViewModel Map(Item fromObject, ItemViewModel toObject = null)
        {
            var item = toObject ?? new ItemViewModel();

            item.CreatedDate = fromObject.CreatedDate;
            item.Description = fromObject.Description;
            item.Id = fromObject.Id;
            item.ImagePath = fromObject.ImagePath;
            item.ItemCategory = fromObject.ItemCategory;
            item.ItemDiscontinued = fromObject.ItemDiscontinued;
            item.ModifiedDate = fromObject.ModifiedDate;
            item.Name = fromObject.Name;
            item.Price = fromObject.Price;

            return item;
        }

        public Item Map(ItemViewModel fromObject, Item toObject = null)
        {
            var item = toObject ?? new Item();

            item.CreatedDate = fromObject.CreatedDate == DateTime.MinValue ? DateTime.UtcNow : fromObject.CreatedDate;
            item.Description = fromObject.Description;
            item.Id = fromObject.Id;
            item.ImagePath = fromObject.ImagePath;
            item.ItemCategory = fromObject.ItemCategory;
            item.ItemDiscontinued = fromObject.ItemDiscontinued;
            item.ModifiedDate = DateTime.UtcNow;
            item.Name = fromObject.Name;
            item.Price = fromObject.Price;

            return item;
        }
    }
}
