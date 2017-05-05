using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoelScottFitness.Services.Services
{
    public interface IJSFitnessService
    {
        Task<AsyncResult<long>> CreateOrUpdateBlog(BlogViewModel blog);

        Task<BlogViewModel> GetBlog(long id);

        Task<IEnumerable<BlogViewModel>> GetBlogs(int number = 0, bool activeOnly = true);

        Task<bool> DeactivateBlog(long id);

        Task<AsyncResult<long>> CreateOrUpdateCustomer(CustomerViewModel customer);

        Task<CustomerViewModel> GetCustomerDetails(long id);

        Task<AsyncResult<long>> CreateOrUpdateDiscountCode(DiscountCodeViewModel discountCode);

        Task<DiscountCodeViewModel> GetDiscountCode(long id);

        Task<IEnumerable<DiscountCodeViewModel>> GetDiscountCodes();

        Task<AsyncResult<long>> CreateOrUpdatePlan(PlanViewModel plan);

        Task<PlanViewModel> GetPlan(long id);

        Task<IEnumerable<PlanViewModel>> GetPlans();

        Task<bool> DeactivatePlan(long id);

        Task<AsyncResult<long>> CreatePurchase(PurchaseViewModel purchase);

        Task<PurchaseViewModel> GetPurchase(long id);

        Task<IEnumerable<PurchaseViewModel>> GetPurchases(long customerId);
    }
}
