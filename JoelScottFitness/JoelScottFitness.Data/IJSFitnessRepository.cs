using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoelScottFitness.Data
{
    public interface IJSFitnessRepository
    {
        Task<AsyncResult<long>> CreateOrUpdateBlogAsync(Blog blog);

        Task<Blog> GetBlogAsync(long id);

        Task<IEnumerable<Blog>> GetBlogsAsync(int number = 0, bool activeOnly = true);

        Task<bool> DeactivateBlogAsync(long id);

        Task<AsyncResult<long>> CreateCustomerAsync(Customer customer);

        Task<AsyncResult<long>> UpdateCustomerAsync(Customer customer);

        Task<Customer> GetCustomerDetailsAsync(long id);

        Task<Customer> GetCustomerDetailsAsync(string userName);

        Task<AsyncResult<long>> CreateOrUpdateDiscountCodeAsync(DiscountCode discountCode);

        Task<DiscountCode> GetDiscountCodeAsync(long id);

        Task<IEnumerable<DiscountCode>> GetDiscountCodesAsync();

        Task<AsyncResult<long>> CreateOrUpdatePlanAsync(Plan plan);

        Task<Plan> GetPlanAsync(long id);

        Task<PlanOption> GetPlanOptionAsync(long id);

        Task<IEnumerable<Plan>> GetPlansAsync();

        Task<IEnumerable<Plan>> GetPlansByGenderAsync(Gender gender);

        Task<bool> DeactivatePlanAsync(long id);

        Task<AsyncResult<long>> CreatePurchaseAsync(Purchase purchase);

        Task<Purchase> GetPurchaseAsync(long id);

        Task<IEnumerable<Purchase>> GetPurchasesAsync(long customerId);

        Task<bool> UpdateMailingList(MailingListItem mailingListItem);

        Task<IEnumerable<PlanOption>> GetBasketItems(IEnumerable<long> ids);

        Task<AuthUser> GetUser(string userName);
    }
}
