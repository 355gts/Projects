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

        Task<AsyncResult<long>> CreateDiscountCodeAsync(CreateDiscountCodeViewModel discountCode);

        Task<AsyncResult<long>> UpdateDiscountCodeAsync(DiscountCodeViewModel discountCode);

        Task<DiscountCodeViewModel> GetDiscountCodeAsync(long id);

        Task<IEnumerable<DiscountCodeViewModel>> GetDiscountCodesAsync();

        Task<AsyncResult<long>> CreatePlanAsync(CreatePlanViewModel plan);

        Task<AsyncResult<long>> UpdatePlanAsync(PlanViewModel plan);
        
        Task<PlanViewModel> GetPlanAsync(long id);

        Task<IEnumerable<PlanViewModel>> GetPlansAsync();

        Task<IEnumerable<UiPlanViewModel>> GetPlansByGenderAsync(Gender gender);

        Task<PlanOptionViewModel> GetPlanOptionAsync(long id);
                
        Task<PurchaseHistoryViewModel> GetPurchaseAsync(long id);

        Task<IEnumerable<PurchaseSummaryViewModel>> GetPurchaseSummaryAsync(long customerId);

        Task<IEnumerable<PurchaseSummaryViewModel>> GetPurchasesAsync();

        PaymentInitiationResult InitiatePayPalPayment(ConfirmPurchaseViewModel confirmPurchaseViewModel, string baseUri);

        PaymentResult CompletePayPalPayment(string paymentId, string payerId);

        Task<bool> UpdateMailingListAsync(MailingListItemViewModel mailingListItem);
        
        Task<IEnumerable<SelectedPlanOptionViewModel>> GetBasketItemsAsync(IEnumerable<long> ids);

        Task<UserViewModel> GetUserAsync(string userName);

        Task<AsyncResult<long>> SavePurchaseAsync(ConfirmPurchaseViewModel confirmPurchaseViewModel);

        Task<bool> UpdatePurchaseStatusAsync(string transactionId, PurchaseStatus status);

        Task<PurchaseHistoryViewModel> GetPurchaseByTransactionIdAsync(string transactionId);

        Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(QuestionnaireViewModel questionnaire);

        Task<QuestionnaireViewModel> GetQuestionnaireAsync(long questionnaireId);

        Task<bool> UpdatePlanStatusAsync(long planId, bool status);

        Task<bool> UpdateBlogStatusAsync(long blogId, bool status);

        Task<AsyncResult<long>> AddImage(string imagePath);

        Task<ImageListViewModel> GetImages();

        Task<AsyncResult<long>> CreateOrUpdateImageConfiguration(ImageConfigurationViewModel imageConfiguration);

        Task<ImageConfigurationViewModel> GetImageConfiguration();

        Task<SectionImageViewModel> GetSectionImages();

        Task<bool> AssociatePlanToPurchase(long purchasedItemId, string planPath);

        Task<IEnumerable<PurchasedHistoryItemViewModel>> GetCustomerPlansAsync(long customerId);

        Task<KaleidoscopeViewModel> GetKaleidoscopeImages();

        Task<bool> UploadHallOfFameAsync(long purchasedItemId, string beforeImage, string afterImage, string comment);

        Task<IEnumerable<HallOfFameViewModel>> GetHallOfFameEntries(bool onlyEnabled = true);
    }
}
