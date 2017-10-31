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
        Task<AsyncResult<long>> CreateOrUpdateBlog(BlogViewModel blog);

        Task<BlogViewModel> GetBlog(long id);

        Task<IEnumerable<BlogViewModel>> GetBlogs(int number = 0, bool activeOnly = true);

        Task<bool> DeactivateBlog(long id);

        Task<AsyncResult<long>> CreateCustomer(CreateCustomerViewModel customer);

        Task<AsyncResult<long>> UpdateCustomer(CustomerViewModel customer);

        Task<CustomerViewModel> GetCustomerDetails(long id);

        Task<CustomerViewModel> GetCustomerDetails(string userName);

        Task<AsyncResult<long>> CreateOrUpdateDiscountCode(DiscountCodeViewModel discountCode);

        Task<DiscountCodeViewModel> GetDiscountCode(long id);

        Task<IEnumerable<DiscountCodeViewModel>> GetDiscountCodes();

        Task<AsyncResult<long>> CreateOrUpdatePlan(PlanViewModel plan);

        Task<PlanViewModel> GetPlan(long id);

        Task<IEnumerable<PlanViewModel>> GetPlans();

        Task<IEnumerable<PlanViewModel>> GetPlansByGender(Gender gender);

        Task<PlanOptionViewModel> GetPlanOptionAsync(long id);

        Task<bool> DeactivatePlan(long id);
        
        Task<PurchaseHistoryViewModel> GetPurchase(long id);

        Task<IEnumerable<PurchaseHistoryViewModel>> GetPurchases(long customerId);

        PaymentInitiationResult InitiatePayPalPayment(ConfirmPurchaseViewModel confirmPurchaseViewModel, string baseUri);

        PaymentResult CompletePayPalPayment(string paymentId, string payerId);

        Task<bool> UpdateMailingList(MailingListItemViewModel mailingListItem);
        
        Task<IEnumerable<PlanOptionViewModel>> GetBasketItems(IEnumerable<long> ids);

        Task<UserViewModel> GetUser(string userName);

        Task<AsyncResult<long>> SavePurchase(ConfirmPurchaseViewModel confirmPurchaseViewModel);

        Task<bool> UpdatePurchaseStatus(string transactionId, PurchaseStatus status);

        Task<long?> GetPurchaseIdByTransactionId(string transactionId);

        Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(QuestionnaireViewModel questionnaire);

        Task<QuestionnaireViewModel> GetQuestionnaireAsync(long questionnaireId);

    }
}
