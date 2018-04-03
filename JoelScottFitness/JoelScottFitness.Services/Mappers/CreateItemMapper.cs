using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateItemMapper : ITypeMapper<CreateItemViewModel, Item>
    {
        public Item Map(CreateItemViewModel fromObject, Item toObject = null)
        {
            var item = toObject ?? new Item();

            item.CreatedDate = DateTime.UtcNow;
            item.Description = fromObject.Description;
            item.ImagePath = fromObject.ImagePath;
            item.ItemCategory = fromObject.ItemCategory;
            item.Name = fromObject.Name;
            item.Price = fromObject.Price;

            return item;
        }
    }
}
