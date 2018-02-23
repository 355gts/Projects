using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Enumerations;
using System;
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
        
        Task<AsyncResult<Guid>> CreateCustomerAsync(CreateCustomerViewModel customer, long? userId = null);

        Task<AsyncResult<Guid>> UpdateCustomerAsync(CustomerViewModel customer);

        Task<CustomerViewModel> GetCustomerDetailsAsync(Guid id);

        Task<CustomerViewModel> GetCustomerDetailsAsync(string userName);

        Task<AsyncResult<long>> CreateDiscountCodeAsync(CreateDiscountCodeViewModel discountCode);

        Task<AsyncResult<long>> UpdateDiscountCodeAsync(DiscountCodeViewModel discountCode);

        Task<DiscountCodeViewModel> GetDiscountCodeAsync(long id);

        Task<DiscountCodeViewModel> GetDiscountCodeAsync(string code);

        Task<IEnumerable<DiscountCodeViewModel>> GetDiscountCodesAsync();

        Task<AsyncResult<long>> CreatePlanAsync(CreatePlanViewModel plan);

        Task<AsyncResult<long>> UpdatePlanAsync(PlanViewModel plan);
        
        Task<PlanViewModel> GetPlanAsync(long id);

        Task<IEnumerable<PlanViewModel>> GetPlansAsync();

        Task<IEnumerable<UiPlanViewModel>> GetPlansByGenderAsync(Gender gender);

        Task<PlanOptionViewModel> GetPlanOptionAsync(long id);
                
        Task<PurchaseHistoryViewModel> GetPurchaseAsync(long id);

        Task<IEnumerable<PurchaseSummaryViewModel>> GetPurchaseSummaryAsync(Guid customerId);

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

        Task<AsyncResult<long>> AddImageAsync(string imagePath);

        Task<ImageListViewModel> GetImagesAsync();

        Task<AsyncResult<long>> CreateOrUpdateImageConfigurationAsync(ImageConfigurationViewModel imageConfiguration);

        Task<ImageConfigurationViewModel> GetImageConfigurationAsync();

        Task<SectionImageViewModel> GetSectionImagesAsync();

        Task<bool> AssociatePlanToPurchaseAsync(long purchasedItemId, string planPath);

        Task<IEnumerable<PurchasedHistoryItemViewModel>> GetCustomerPlansAsync(Guid customerId);

        Task<KaleidoscopeViewModel> GetKaleidoscopeImagesAsync();

        Task<bool> UploadHallOfFameAsync(long purchasedItemId, string beforeImage, string afterImage, string comment);

        Task<IEnumerable<HallOfFameViewModel>> GetHallOfFameEntriesAsync(bool onlyEnabled = true, int? numberOfEntries = null);

        Task<bool> UpdateHallOfFameStatusAsync(long purchasedItemId, bool status);
        Task<bool> DeleteHallOfFameEntryAsync(long purchasedItemId);

        Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers);

        Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers, IEnumerable<string> attachmentPaths);
    }
}
