using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
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

        Task<OrderHistoryViewModel> GetOrderAsync(long id);

        Task<IEnumerable<OrderSummaryViewModel>> GetOrderSummaryAsync(Guid customerId);

        Task<IEnumerable<OrderSummaryViewModel>> GetOrdersAsync();

        PaymentInitiationResult InitiatePayPalPayment(ConfirmOrderViewModel confirmOrderViewModel, string baseUri);

        PaymentResult CompletePayPalPayment(string paymentId, string payerId);

        Task<bool> UpdateMailingListAsync(MailingListItemViewModel mailingListItem);

        Task<UserViewModel> GetUserAsync(string userName);

        Task<AsyncResult<long>> SaveOrderAsync(ConfirmOrderViewModel confirmOrderViewModel);

        Task<bool> UpdateOrderStatusAsync(string transactionId, OrderStatus status);

        Task<OrderHistoryViewModel> GetOrderByOrderIdAsync(long orderId);

        Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(QuestionnaireViewModel questionnaire);

        Task<QuestionnaireViewModel> GetQuestionnaireAsync(long questionnaireId);

        Task<bool> UpdatePlanStatusAsync(long planId, bool status);

        Task<bool> UpdateBlogStatusAsync(long blogId, bool status);

        Task<AsyncResult<long>> AddImageAsync(string imagePath);

        Task<bool> DeleteImageAsync(long imageId);

        Task<ImageListViewModel> GetImagesAsync();

        Task<AsyncResult<long>> CreateOrUpdateImageConfigurationAsync(ImageConfigurationViewModel imageConfiguration);

        Task<ImageConfigurationViewModel> GetImageConfigurationAsync();

        Task<SectionImageViewModel> GetSectionImagesAsync();

        Task<bool> UploadCustomerPlanAsync(long planId, string planPath);

        Task<IEnumerable<CustomerPlanViewModel>> GetCustomerPlansAsync(Guid customerId);

        Task<KaleidoscopeViewModel> GetKaleidoscopeImagesAsync();

        Task<bool> UploadHallOfFameAsync(long orderId, string beforeImage, string afterImage, string comment);

        Task<IEnumerable<HallOfFameViewModel>> GetHallOfFameEntriesAsync(bool onlyEnabled = true, int? numberOfEntries = null);

        Task<bool> UpdateHallOfFameStatusAsync(long orderId, bool status);
        Task<bool> DeleteHallOfFameEntryAsync(long orderId);

        Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers);

        Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers, IEnumerable<string> attachmentPaths);

        Task<AsyncResult<long>> CreateMessageAsync(CreateMessageViewModel message);

        Task<AsyncResult<long>> UpdateMessageAsync(MessageViewModel message);

        Task<IEnumerable<MessageViewModel>> GetMessagesAsync();

        Task<MessageViewModel> GetMessageAsync(long id);

        Task<AsyncResult<long>> CreateCustomerPlanAsync(CreateCustomerPlanViewModel customerPlan);

        Task<AsyncResult<long>> UpdateCustomerPlanAsync(CustomerPlanViewModel customerPlan);

        Task<IEnumerable<CustomerPlanViewModel>> GetCustomerPlansForOrderAsync(long orderId);

        Task<bool> MarkOrderCompleteAsync(long orderId);
    }
}
