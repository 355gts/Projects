using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class AddressMapper : ITypeMapper<Address, AddressViewModel>, ITypeMapper<AddressViewModel, Address>
    {
        public AddressViewModel Map(Address fromObject, AddressViewModel toObject = null)
        {
            var address = toObject ?? new AddressViewModel();

            address.Id = fromObject.Id;
            address.AddressLine1 = fromObject.AddressLine1;
            address.AddressLine2 = fromObject.AddressLine2;
            address.AddressLine3 = fromObject.AddressLine3;
            address.City = fromObject.City;
            address.PostCode = fromObject.PostCode;
            address.Region = fromObject.Region;

            return address;
        }

        public Address Map(AddressViewModel fromObject, Address toObject = null)
        {
            var address = toObject ?? new Address();

            address.Id = fromObject.Id;
            address.AddressLine1 = fromObject.AddressLine1;
            address.AddressLine2 = fromObject.AddressLine2;
            address.AddressLine3 = fromObject.AddressLine3;
            address.City = fromObject.City;
            address.PostCode = fromObject.PostCode;
            address.Region = fromObject.Region;

            return address;
        }
    }
}
