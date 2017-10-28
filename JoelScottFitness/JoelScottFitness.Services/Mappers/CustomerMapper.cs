using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System.Collections.Generic;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CustomerMapper : ITypeMapper<CustomerViewModel, Customer>, ITypeMapper<Customer, CustomerViewModel>
    {
        AddressMapper addressMapper = new AddressMapper();
        PurchaseMapper purchaseMapper = new PurchaseMapper();
        UserMapper userMapper = new UserMapper();

        public Customer Map(CustomerViewModel fromObject, Customer toObject = null)
        {
            var customer = toObject ?? new Customer();

            customer.CreatedDate = fromObject.CreatedDate;
            customer.EmailAddress = fromObject.EmailAddress;
            customer.Firstname = fromObject.Firstname;
            customer.Id = fromObject.Id;
            customer.ModifiedDate = fromObject.ModifiedDate;
            customer.Surname = fromObject.Surname;
            customer.UserId = fromObject.UserId;
            customer.BillingAddressId = fromObject.BillingAddressId;

            if (fromObject.BillingAddress != null)
                customer.BillingAddress = addressMapper.Map(fromObject.BillingAddress);

            if (fromObject.PurchaseHistory != null)
            {
                var purchases = new List<Purchase>();
                foreach (var item in fromObject.PurchaseHistory)
                {
                    purchases.Add(purchaseMapper.Map(item));
                }

                customer.PurchaseHistory = purchases;
            }

            return customer;
        }

        public CustomerViewModel Map(Customer fromObject, CustomerViewModel toObject = null)
        {
            var customer = toObject ?? new CustomerViewModel();

            customer.CreatedDate = fromObject.CreatedDate;
            customer.EmailAddress = fromObject.EmailAddress;
            customer.Firstname = fromObject.Firstname;
            customer.Id = fromObject.Id;
            customer.ModifiedDate = fromObject.ModifiedDate;
            customer.Surname = fromObject.Surname;
            customer.UserId = fromObject.UserId;
            customer.BillingAddressId = fromObject.BillingAddressId;

            if (fromObject.BillingAddress != null)
                customer.BillingAddress = addressMapper.Map(fromObject.BillingAddress);

            if (fromObject.User != null)
                customer.User = userMapper.Map(fromObject.User);

            if (fromObject.PurchaseHistory != null)
            {
                var purchases = new List<PurchaseHistoryViewModel>();
                foreach (var item in fromObject.PurchaseHistory)
                {
                    purchases.Add(purchaseMapper.Map(item));
                }

                customer.PurchaseHistory = purchases;
            }

            return customer;
        }
    }
}
