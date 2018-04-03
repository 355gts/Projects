using JoelScottFitness.Common.Enumerations;
using System;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class BasketItemViewModel
    {
        [DataMember(IsRequired = true)]
        public long Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public Double Price { get; set; }

        [DataMember(IsRequired = true)]
        public bool ItemDiscounted { get; set; }

        [DataMember(IsRequired = false)]
        public Double DiscountedPrice { get { return ItemDiscounted ? Math.Round(Price - (Price / 100 * DiscountPercent), 2) : 0; } }

        [DataMember(IsRequired = false)]
        public int DiscountPercent { get; set; }

        [DataMember(IsRequired = true)]
        public long Quantity { get; set; }

        [DataMember(IsRequired = true)]
        public double ItemTotal
        {
            get
            {
                return ItemDiscounted
                        ? Math.Round(DiscountedPrice * Quantity, 2)
                        : Math.Round(Price * Quantity, 2);
            }
        }

        public ItemCategory ItemCategory { get; set; }
    }
}
