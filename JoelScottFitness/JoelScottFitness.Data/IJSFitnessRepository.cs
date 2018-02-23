using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Enumerations;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoelScottFitness.Data
{
    public interface IJSFitnessRepository
    {
        Task<AsyncResult<long>> CreateOrUpdateBlogAsync(Blog blog);

        Task<Blog> GetBlogAsync(long id);

        Task<IEnumerable<Blog>> GetBlogsAsync(int number = 0);

        Task<AsyncResult<Guid>> CreateCustomerAsync(Customer customer);

        Task<AsyncResult<Guid>> UpdateCustomerAsync(Customer customer);

        Task<Customer> GetCustomerDetailsAsync(Guid id);

        Task<Customer> GetCustomerDetailsAsync(string userName);

        Task<AsyncResult<long>> CreateOrUpdateDiscountCodeAsync(DiscountCode discountCode);

        Task<DiscountCode> GetDiscountCodeAsync(long id);

        Task<DiscountCode> GetDiscountCodeAsync(string code);

        Task<IEnumerable<DiscountCode>> GetDiscountCodesAsync();

        Task<AsyncResult<long>> CreateOrUpdatePlanAsync(Plan plan);

        Task<Plan> GetPlanAsync(long id);

        Task<PlanOption> GetPlanOptionAsync(long id);

        Task<IEnumerable<Plan>> GetPlansAsync();

        Task<IEnumerable<Plan>> GetPlansByGenderAsync(Gender gender);

        Task<Purchase> GetPurchaseAsync(long id);

        Task<IEnumerable<Purchase>> GetPurchasesAsync(Guid customerId);

        Task<IEnumerable<Purchase>> GetPurchasesAsync();

        Task<bool> UpdateMailingListAsync(MailingListItem mailingListItem);

        Task<IEnumerable<PlanOption>> GetBasketItemsAsync(IEnumerable<long> ids);

        Task<AuthUser> GetUserAsync(string userName);

        Task<AsyncResult<long>> SavePurchaseAsync(Purchase purchase);

        Task<bool> UpdatePurchaseStatusAsync(string transactionId, PurchaseStatus status);

        Task<Purchase> GetPurchaseByTransactionIdAsync(string transactionId);

        Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(Questionnaire questionnaire);

        Task<Questionnaire> GetQuestionnaireAsync(long questionnaireId);

        Task<bool> UpdatePlanStatusAsync(long planId, bool status);

        Task<bool> UpdateBlogStatusAsync(long blogId, bool status);

        Task<bool> AssociateQuestionnaireToPurchaseAsync(long purchaseId, long questionnaireId);

        Task<AsyncResult<long>> AddImageAsync(Image image);

        Task<IEnumerable<Image>> GetImagesAsync();

        Task<AsyncResult<long>> CreateOrUpdateImageConfigurationAsync(ImageConfiguration imageConfiguration);

        Task<ImageConfiguration> GetImageConfigurationAsync();

        Task<bool> AssociatePlanToPurchaseAsync(long purchasedItemId, string planPath);

        Task<IEnumerable<PlanOption>> GetPlanOptionsAsync();

        Task<PurchasedItem> GetPurchasedItemAsync(long purchasedItemId);

        Task<bool> UpdatePurchasedItemAsync(PurchasedItem purchasedItem);

        Task<IEnumerable<PurchasedItem>> GetHallOfFameEntriesAsync(bool onlyEnabled = true, int? numberOfEntries = null);

        Task<bool> UpdateHallOfFameStatusAsync(long purchasedItemId, bool status);

        Task<bool> DeleteHallOfFameEntryAsync(long purchasedItemId);

        Task<AsyncResult<long>> CreateOrUpdateMessageAsync(Message message);

        Task<IEnumerable<Message>> GetMessagesAsync();

        Task<Message> GetMessageAsync(long id);
    }
}
