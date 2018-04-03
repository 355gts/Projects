using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateCustomerPlanMapper : ITypeMapper<CreateCustomerPlanViewModel, CustomerPlan>
    {
        public CustomerPlan Map(CreateCustomerPlanViewModel fromObject, CustomerPlan toObject = null)
        {
            var customerPlan = toObject ?? new CustomerPlan();

            customerPlan.CustomerId = fromObject.CustomerId;
            customerPlan.ItemId = fromObject.ItemId;
            customerPlan.OrderId = fromObject.OrderId;

            return customerPlan;
        }
    }
}
