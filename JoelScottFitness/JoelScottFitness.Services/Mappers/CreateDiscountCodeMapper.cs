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
    sealed class CreateDiscountCodeMapper : ITypeMapper<CreateDiscountCodeViewModel, DiscountCode>
    {
        public DiscountCode Map(CreateDiscountCodeViewModel fromObject, DiscountCode toObject = null)
        {
            var discountCode = toObject ?? new DiscountCode();

            discountCode.Code = fromObject.Code;
            discountCode.PercentDiscount = fromObject.PercentDiscount;
            discountCode.ValidFrom = fromObject.ValidFrom;
            discountCode.ValidTo = fromObject.ValidTo;

            return discountCode;
        }
    }
}
