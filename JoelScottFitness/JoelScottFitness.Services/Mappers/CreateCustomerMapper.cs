using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateCustomerMapper : ITypeMapper<CreateCustomerViewModel, Customer>
    {
        AddressMapper addressMapper = new AddressMapper();
        PurchaseMapper purchaseMapper = new PurchaseMapper();

        public Customer Map(CreateCustomerViewModel fromObject, Customer toObject = null)
        {
            var customer = toObject ?? new Customer();

            customer.EmailAddress = fromObject.EmailAddress;
            customer.Firstname = fromObject.Firstname;
            customer.Surname = fromObject.Surname;
            customer.UserId = fromObject.UserId;

            if (fromObject.BillingAddress != null)
                customer.BillingAddress = addressMapper.Map(fromObject.BillingAddress);

            return customer;
        }
    }
}
