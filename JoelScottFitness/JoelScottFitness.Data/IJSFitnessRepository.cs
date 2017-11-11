using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Enumerations;
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

        Task<IEnumerable<Blog>> GetBlogsAsync(int number = 0);
        
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
        
        Task<Purchase> GetPurchaseAsync(long id);

        Task<IEnumerable<Purchase>> GetPurchasesAsync(long customerId);

        Task<IEnumerable<Purchase>> GetPurchasesAsync();

        Task<bool> UpdateMailingListAsync(MailingListItem mailingListItem);

        Task<IEnumerable<PlanOption>> GetBasketItemsAsync(IEnumerable<long> ids);

        Task<AuthUser> GetUserAsync(string userName);

        Task<AsyncResult<long>> SavePurchaseAsync(Purchase purchase);

        Task<bool> UpdatePurchaseStatus(string transactionId, PurchaseStatus status);

        Task<Purchase> GetPurchaseByTransactionId(string transactionId);

        Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(Questionnaire questionnaire);

        Task<Questionnaire> GetQuestionnaireAsync(long questionnaireId);

        Task<bool> UpdatePlanStatusAsync(long planId, bool status);

        Task<bool> UpdateBlogStatusAsync(long blogId, bool status);

        Task<bool> AssociateQuestionnaireToPurchase(long purchaseId, long questionnaireId);

        Task<AsyncResult<long>> AddImage(Image image);

        Task<IEnumerable<Image>> GetImages();

        Task<AsyncResult<long>> CreateOrUpdateImageConfiguration(ImageConfiguration imageConfiguration);
    }
}
