using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Results;
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

        Task<Order> GetOrdersAsync(long id);

        Task<IEnumerable<Order>> GetPurchasesAsync(Guid customerId);

        Task<IEnumerable<Order>> GetPurchasesAsync();

        Task<bool> UpdateMailingListAsync(MailingListItem mailingListItem);

        Task<IEnumerable<PlanOption>> GetBasketItemsAsync(IEnumerable<long> ids);

        Task<AuthUser> GetUserAsync(string userName);

        Task<AsyncResult<long>> SavePurchaseAsync(Order purchase);

        Task<bool> UpdatePurchaseStatusAsync(string transactionId, PurchaseStatus status);

        Task<Order> GetPurchaseByOrderIdAsync(long orderId);

        Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(Questionnaire questionnaire);

        Task<Questionnaire> GetQuestionnaireAsync(long questionnaireId);

        Task<bool> UpdatePlanStatusAsync(long planId, bool status);

        Task<bool> UpdateBlogStatusAsync(long blogId, bool status);

        Task<bool> AssociateQuestionnaireToPurchaseAsync(long purchaseId, long questionnaireId);

        Task<bool> AssociateQuestionnaireToPlansAsync(long orderId);

        Task<AsyncResult<long>> AddImageAsync(Image image);

        Task<bool> DeleteImageAsync(long imageId);

        Task<IEnumerable<Image>> GetImagesAsync();

        Task<AsyncResult<long>> CreateOrUpdateImageConfigurationAsync(ImageConfiguration imageConfiguration);

        Task<ImageConfiguration> GetImageConfigurationAsync();

        Task<bool> UploadCustomerPlanAsync(long purchasedItemId, string planPath);

        Task<IEnumerable<PlanOption>> GetPlanOptionsAsync();

        Task<OrderItem> GetPurchasedItemAsync(long purchasedItemId);

        Task<bool> UpdatePurchasedItemAsync(OrderItem purchasedItem);

        Task<IEnumerable<CustomerPlan>> GetHallOfFameEntriesAsync(bool onlyEnabled = true, int? numberOfEntries = null);

        Task<bool> UpdateHallOfFameStatusAsync(long customerPlanId, bool status);

        Task<bool> DeleteHallOfFameEntryAsync(long customerPlanId);

        Task<AsyncResult<long>> CreateOrUpdateMessageAsync(Message message);

        Task<IEnumerable<Message>> GetMessagesAsync();

        Task<Message> GetMessageAsync(long id);

        Task<AsyncResult<long>> CreateCustomerPlanAsync(CustomerPlan customerPlan);

        Task<AsyncResult<long>> UpdateCustomerPlanAsync(CustomerPlan customerPlan);

        Task<IEnumerable<CustomerPlan>> GetCustomerPlansForPurchaseAsync(long orderId);

        Task<IEnumerable<CustomerPlan>> GetCustomerPlansAsync(Guid customerId);

        Task<CustomerPlan> GetCustomerPlanAsync(long customerPlanId);

        Task<bool> UpdateHallOfFameDetailsAsync(CustomerPlan customerPlan);
    }
}
