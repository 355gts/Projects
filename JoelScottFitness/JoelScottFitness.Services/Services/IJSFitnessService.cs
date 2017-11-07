using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Enumerations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoelScottFitness.Services.Services
{
    public interface IJSFitnessService
    {
        Task<AsyncResult<long>> CreateBlogAsync(CreateBlogViewModel blog);

        Task<AsyncResult<long>> UpdateBlogAsync(BlogViewModel blog);

        Task<BlogViewModel> GetBlogAsync(long id);

        Task<IEnumerable<BlogViewModel>> GetBlogsAsync(int number = 0);
        
        Task<AsyncResult<long>> CreateCustomerAsync(CreateCustomerViewModel customer);

        Task<AsyncResult<long>> UpdateCustomerAsync(CustomerViewModel customer);

        Task<CustomerViewModel> GetCustomerDetailsAsync(long id);

        Task<CustomerViewModel> GetCustomerDetailsAsync(string userName);

        Task<AsyncResult<long>> CreateOrUpdateDiscountCodeAsync(DiscountCodeViewModel discountCode);

        Task<DiscountCodeViewModel> GetDiscountCodeAsync(long id);

        Task<IEnumerable<DiscountCodeViewModel>> GetDiscountCodesAsync();

        Task<AsyncResult<long>> CreatePlanAsync(CreatePlanViewModel plan);

        Task<AsyncResult<long>> UpdatePlanAsync(PlanViewModel plan);
        
        Task<PlanViewModel> GetPlanAsync(long id);

        Task<IEnumerable<PlanViewModel>> GetPlansAsync();

        Task<IEnumerable<UiPlanViewModel>> GetPlansByGenderAsync(Gender gender);

        Task<PlanOptionViewModel> GetPlanOptionAsync(long id);
                
        Task<PurchaseHistoryViewModel> GetPurchaseAsync(long id);

        Task<IEnumerable<PurchaseHistoryViewModel>> GetPurchasesAsync(long customerId);

        PaymentInitiationResult InitiatePayPalPayment(ConfirmPurchaseViewModel confirmPurchaseViewModel, string baseUri);

        PaymentResult CompletePayPalPayment(string paymentId, string payerId);

        Task<bool> UpdateMailingListAsync(MailingListItemViewModel mailingListItem);
        
        Task<IEnumerable<SelectedPlanOptionViewModel>> GetBasketItemsAsync(IEnumerable<long> ids);

        Task<UserViewModel> GetUserAsync(string userName);

        Task<AsyncResult<long>> SavePurchaseAsync(ConfirmPurchaseViewModel confirmPurchaseViewModel);

        Task<bool> UpdatePurchaseStatusAsync(string transactionId, PurchaseStatus status);

        Task<long?> GetPurchaseIdByTransactionIdAsync(string transactionId);

        Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(QuestionnaireViewModel questionnaire);

        Task<QuestionnaireViewModel> GetQuestionnaireAsync(long questionnaireId);

        Task<bool> UpdatePlanStatusAsync(long planId, bool status);

        Task<bool> UpdateBlogStatusAsync(long blogId, bool status);
    }
}
