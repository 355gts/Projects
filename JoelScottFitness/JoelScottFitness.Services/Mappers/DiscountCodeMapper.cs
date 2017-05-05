using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoelScottFitness.Services.Mappers
{
    sealed class DiscountCodeMapper : ITypeMapper<DiscountCode, DiscountCodeViewModel>, ITypeMapper<DiscountCodeViewModel, DiscountCode>
    {
        public DiscountCodeViewModel Map(DiscountCode fromObject, DiscountCodeViewModel toObject = null)
        {
            var discountCode = toObject ?? new DiscountCodeViewModel();

            discountCode.Active = fromObject.Active;
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
