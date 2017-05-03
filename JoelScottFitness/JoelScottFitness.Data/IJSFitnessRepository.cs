using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoelScottFitness.Data
{
    public interface IJSFitnessRepository
    {
        Task<AsyncResult<long>> CreateOrUpdateBlog(Blog blog);

        Task<Blog> GetBlog(long id);

        Task<IEnumerable<Blog>> GetBlogs(int number = 0, bool activeOnly = true);

        Task<bool> DeactivateBlog(long id);

        Task<AsyncResult<long>> CreateOrUpdateCustomer(Customer customer);

        Task<Customer> GetCustomerDetails(long id);

        Task<AsyncResult<long>> CreateOrUpdateDiscountCode(DiscountCode discountCode);

        Task<DiscountCode> GetDiscountCode(long id);

        Task<IEnumerable<DiscountCode>> GetDiscountCodes();

        Task<AsyncResult<long>> CreateOrUpdatePlan(Plan plan);

        Task<Plan> GetPlan(long id);

        Task<IEnumerable<Plan>> GetPlans();

        Task<bool> DeactivatePlan(long id);

        Task<AsyncResult<long>> CreatePurchase(Purchase purchase);

        Task<Purchase> GetPurchase(long id);

        Task<IEnumerable<Purchase>> GetPurchases(long customerId);
    }
}
