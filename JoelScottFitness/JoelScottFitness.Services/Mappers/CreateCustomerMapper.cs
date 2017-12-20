using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateCustomerMapper : ITypeMapper<CreateCustomerViewModel, Customer>
    {
        AddressMapper addressMapper = new AddressMapper();

        public Customer Map(CreateCustomerViewModel fromObject, Customer toObject = null)
        {
            var customer = toObject ?? new Customer();

            customer.EmailAddress = fromObject.EmailAddress;
            customer.Firstname = fromObject.Firstname;
            customer.Surname = fromObject.Surname;

            if (fromObject.BillingAddress != null)
                customer.BillingAddress = addressMapper.Map(fromObject.BillingAddress);

            return customer;
        }
    }
}
