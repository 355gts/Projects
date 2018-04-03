using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class BasketViewModel
    {
        [DataMember(IsRequired = false)]
        public DiscountCodeViewModel DiscountCode { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public IDictionary<long, BasketItemViewModel> Items { get; set; }

        [DataMember(IsRequired = false)]
        public int NumberOfItems
        {
            get { return Items == null || !Items.Any() ? 0 : Items.Count(); }
        }

        public double Total
        {
            get
            {
                return Math.Round(Items.Sum(i => i.Value.Price * i.Value.Quantity));
                //var total = Items.Sum(i => i.Value.Price * i.Value.Quantity);
                //return DiscountCode != null
                //    ? Math.Round(total - (total / 100 * DiscountCode.PercentDiscount), 2)
                //    : Math.Round(total, 2);
            }
        }
    }
}
