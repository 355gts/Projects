using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class DiscountCodeMapper : ITypeMapper<DiscountCode, DiscountCodeViewModel>, ITypeMapper<DiscountCodeViewModel, DiscountCode>
    {
        public DiscountCodeViewModel Map(DiscountCode fromObject, DiscountCodeViewModel toObject = null)
        {
            var discountCode = toObject ?? new DiscountCodeViewModel();
            
            discountCode.Code = fromObject.Code;
            discountCode.Id = fromObject.Id;
            discountCode.PercentDiscount = fromObject.PercentDiscount;
            discountCode.ValidFrom = fromObject.ValidFrom;
            discountCode.ValidTo = fromObject.ValidTo;

            return discountCode;
        }

        public DiscountCode Map(DiscountCodeViewModel fromObject, DiscountCode toObject = null)
        {
            var discountCode = toObject ?? new DiscountCode();
            
            discountCode.Code = fromObject.Code;
            discountCode.Id = fromObject.Id;
            discountCode.PercentDiscount = fromObject.PercentDiscount;
            discountCode.ValidFrom = fromObject.ValidFrom;
            discountCode.ValidTo = fromObject.ValidTo;

            return discountCode;
        }
    }
}
