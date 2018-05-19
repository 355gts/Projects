using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CustomerPlanMapper : ITypeMapper<CustomerPlan, CustomerPlanViewModel>
    {
        public CustomerPlanViewModel Map(CustomerPlan fromObject, CustomerPlanViewModel toObject = null)
        {
            var customerPlan = toObject ?? new CustomerPlanViewModel();

            customerPlan.CustomerId = fromObject.CustomerId;
            customerPlan.Description = fromObject.Item?.Description;
            customerPlan.Id = fromObject.Id;
            customerPlan.ImagePath = fromObject.Item?.ImagePath;
            customerPlan.ItemId = fromObject.ItemId;
            customerPlan.Name = fromObject.Item?.Name;
            customerPlan.OrderId = fromObject.OrderId;
            customerPlan.PlanPath = fromObject.PlanPath;
            customerPlan.QuestionnaireComplete = fromObject.QuestionnaireComplete;
            customerPlan.RequiresAction = fromObject.RequiresAction;
            customerPlan.SheetsUri = fromObject.SheetsUri;

            return customerPlan;
        }
    }
}
