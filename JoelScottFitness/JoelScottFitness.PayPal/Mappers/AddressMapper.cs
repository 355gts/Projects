using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using PayPal.Api;

namespace JoelScottFitness.PayPal.Mappers
{
    sealed class AddressMapper : ITypeMapper<AddressViewModel, Address>
    {
        public Address Map(AddressViewModel fromObject, Address toObject = null)
        {
            var address = toObject ?? new Address();

            address.city = fromObject.City;
            address.country_code = fromObject.CountryCode;
            address.line1 = fromObject.AddressLine1;
            address.line2 = fromObject.AddressLine2;
            address.postal_code = fromObject.PostCode;
            address.state = fromObject.Region;

            return address;
        }
    }
}
