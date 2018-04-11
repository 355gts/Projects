using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity.Models;
using JoelScottFitness.PayPal.Services;
using JoelScottFitness.Services.Properties;
using log4net;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoelScottFitness.Services.Services
{
    public class JSFitnessService : IJSFitnessService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JSFitnessService));

        private readonly IJSFitnessRepository repository;
        private readonly IMapper mapper;
        private readonly IPayPalService paypalService;
        private readonly IEmailService emailService;

        public JSFitnessService(IJSFitnessRepository repository,
                                [Named("ServiceMapper")] IMapper mapper,
                                IPayPalService paypalService,
                                IEmailService emailService)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            if (paypalService == null)
                throw new ArgumentNullException(nameof(paypalService));

            if (emailService == null)
                throw new ArgumentNullException(nameof(paypalService));

            this.repository = repository;
            this.mapper = mapper;
            this.paypalService = paypalService;
            this.emailService = emailService;
        }

        public JSFitnessService()
        {
        }

        public async Task<AsyncResult<long>> CreateBlogAsync(CreateBlogViewModel blog)
        {
            var repoBlog = mapper.Map<CreateBlogViewModel, Blog>(blog);

            return await repository.CreateOrUpdateBlogAsync(repoBlog);
        }

        public async Task<AsyncResult<long>> UpdateBlogAsync(BlogViewModel blog)
        {
            var repoBlog = mapper.Map<BlogViewModel, Blog>(blog);

            return await repository.CreateOrUpdateBlogAsync(repoBlog);
        }


        public async Task<AsyncResult<Guid>> CreateCustomerAsync(CreateCustomerViewModel customer, long? userId = null)
        {
            var repoCustomer = mapper.Map<CreateCustomerViewModel, Customer>(customer);

            if (userId.HasValue)
                repoCustomer.UserId = userId;

            return await repository.CreateCustomerAsync(repoCustomer);
        }

        public async Task<AsyncResult<Guid>> UpdateCustomerAsync(CustomerViewModel customer)
        {
            var repoCustomer = mapper.Map<CustomerViewModel, Customer>(customer);

            return await repository.UpdateCustomerAsync(repoCustomer);
        }

        public async Task<AsyncResult<long>> CreatePlanAsync(CreatePlanViewModel plan)
        {
            var repoPlan = mapper.Map<CreatePlanViewModel, Plan>(plan);

            return await repository.CreateOrUpdatePlanAsync(repoPlan);
        }

        public async Task<AsyncResult<long>> UpdatePlanAsync(PlanViewModel plan)
        {
            var repoPlan = mapper.Map<PlanViewModel, Plan>(plan);

            return await repository.CreateOrUpdatePlanAsync(repoPlan);
        }

        public async Task<BlogViewModel> GetBlogAsync(long id)
        {
            var blog = await repository.GetBlogAsync(id);

            if (blog == null)
                return null;

            return mapper.Map<Blog, BlogViewModel>(blog);
        }

        public async Task<IEnumerable<BlogViewModel>> GetBlogsAsync(int number = 0)
        {
            var blogs = await repository.GetBlogsAsync(number);

            if (blogs == null || !blogs.Any())
                return Enumerable.Empty<BlogViewModel>();

            return mapper.MapEnumerable<Blog, BlogViewModel>(blogs);
        }

        public async Task<CustomerViewModel> GetCustomerDetailsAsync(Guid id)
        {
            var customer = await repository.GetCustomerDetailsAsync(id);

            if (customer == null)
                return null;

            return mapper.Map<Customer, CustomerViewModel>(customer);
        }

        public async Task<CustomerViewModel> GetCustomerDetailsAsync(string userName)
        {
            var customer = await repository.GetCustomerDetailsAsync(userName);

            if (customer == null)
                return null;

            return mapper.Map<Customer, CustomerViewModel>(customer);
        }

        public async Task<DiscountCodeViewModel> GetDiscountCodeAsync(long id)
        {
            var discountCode = await repository.GetDiscountCodeAsync(id);

            if (discountCode == null)
                return null;

            return mapper.Map<DiscountCode, DiscountCodeViewModel>(discountCode);
        }

        public async Task<DiscountCodeViewModel> GetDiscountCodeAsync(string code)
        {
            var discountCode = await repository.GetDiscountCodeAsync(code);

            if (discountCode == null)
                return null;

            return mapper.Map<DiscountCode, DiscountCodeViewModel>(discountCode);
        }

        public async Task<IEnumerable<DiscountCodeViewModel>> GetDiscountCodesAsync()
        {
            var discountCodes = await repository.GetDiscountCodesAsync();

            if (discountCodes == null || !discountCodes.Any())
                return Enumerable.Empty<DiscountCodeViewModel>();

            return mapper.MapEnumerable<DiscountCode, DiscountCodeViewModel>(discountCodes);
        }

        public async Task<PlanViewModel> GetPlanAsync(long id)
        {
            var plan = await repository.GetPlanAsync(id);

            if (plan == null)
                return null;

            return mapper.Map<Plan, PlanViewModel>(plan);
        }

        public async Task<IEnumerable<PlanViewModel>> GetPlansAsync()
        {
            var plans = await repository.GetPlansAsync();

            if (plans == null || !plans.Any())
                return Enumerable.Empty<PlanViewModel>();

            return mapper.MapEnumerable<Plan, PlanViewModel>(plans);
        }

        public async Task<IEnumerable<UiPlanViewModel>> GetPlansByGenderAsync(Gender gender)
        {
            var plans = await repository.GetPlansByGenderAsync(gender);

            if (plans == null || !plans.Any())
                return Enumerable.Empty<UiPlanViewModel>();

            return mapper.MapEnumerable<Plan, UiPlanViewModel>(plans);
        }

        public async Task<PlanOptionViewModel> GetPlanOptionAsync(long id)
        {
            var planOption = await repository.GetPlanOptionAsync(id);

            if (planOption == null)
                return null;

            return mapper.Map<PlanOption, PlanOptionViewModel>(planOption);
        }

        public async Task<OrderHistoryViewModel> GetOrderAsync(long id)
        {
            var order = await repository.GetOrderAsync(id);

            if (order == null)
                return null;

            var orderViewModel = mapper.Map<Order, OrderHistoryViewModel>(order);

            // map these objects like this to avoid circular mapper dependency
            orderViewModel.Customer = mapper.Map<Customer, CustomerViewModel>(order.Customer);

            if (order.DiscountCode != null)
                orderViewModel.DiscountCode = mapper.Map<DiscountCode, DiscountCodeViewModel>(order.DiscountCode);

            if (order.Questionnaire != null)
                orderViewModel.Questionnaire = mapper.Map<Questionnaire, QuestionnaireViewModel>(order.Questionnaire);

            if (order.Customer?.Plans != null)
                orderViewModel.Plans = mapper.MapEnumerable<CustomerPlan, CustomerPlanViewModel>(order.Customer?.Plans);

            return orderViewModel;
        }

        public async Task<IEnumerable<OrderSummaryViewModel>> GetOrderSummaryAsync(Guid customerId)
        {
            var orders = await repository.GetOrdersAsync(customerId);

            if (orders == null || !orders.Any())
                return Enumerable.Empty<OrderSummaryViewModel>();

            return mapper.MapEnumerable<Order, OrderSummaryViewModel>(orders);
        }

        public async Task<IEnumerable<OrderSummaryViewModel>> GetOrdersAsync()
        {
            var orders = await repository.GetOrdersAsync();

            if (orders == null || !orders.Any())
                return Enumerable.Empty<OrderSummaryViewModel>();

            return mapper.MapEnumerable<Order, OrderSummaryViewModel>(orders);
        }

        public async Task<IEnumerable<CustomerPlanViewModel>> GetCustomerPlansAsync(Guid customerId)
        {
            var customerPlans = await repository.GetCustomerPlansAsync(customerId);

            if (customerPlans == null || !customerPlans.Any())
                return Enumerable.Empty<CustomerPlanViewModel>();

            return mapper.MapEnumerable<CustomerPlan, CustomerPlanViewModel>(customerPlans);
        }

        public PaymentInitiationResult InitiatePayPalPayment(ConfirmOrderViewModel confirmOrderViewModel, string baseUri)
        {
            return paypalService.InitiatePayPalPayment(confirmOrderViewModel, baseUri);
        }


        public PaymentResult CompletePayPalPayment(string paymentId, string payerId)
        {
            return paypalService.CompletePayPalPayment(paymentId, payerId);
        }

        public async Task<bool> UpdateMailingListAsync(MailingListItemViewModel mailingListItem)
        {
            var repoMailingListItem = mapper.Map<MailingListItemViewModel, MailingListItem>(mailingListItem);

            return await repository.UpdateMailingListAsync(repoMailingListItem);
        }

        public async Task<UserViewModel> GetUserAsync(string userName)
        {
            var user = await repository.GetUserAsync(userName);

            return mapper.Map<AuthUser, UserViewModel>(user);
        }

        public async Task<AsyncResult<long>> SaveOrderAsync(ConfirmOrderViewModel confirmOrderViewModel)
        {
            var order = mapper.Map<ConfirmOrderViewModel, Order>(fromObject: confirmOrderViewModel);

            var orderResult = await repository.SaveOrderAsync(order);

            // if the order went through create the customer plans
            if (orderResult.Success && order.Items.Any(i => i.ItemCategory == ItemCategory.Plan))
            {
                foreach (var plan in order.Items.Where(i => i.ItemCategory == ItemCategory.Plan).ToList())
                {
                    for (int i = 0; i < plan.Quantity; i++)
                    {
                        var createPlanResult =
                        await CreateCustomerPlanAsync(new CreateCustomerPlanViewModel()
                        {
                            CustomerId = confirmOrderViewModel.CustomerDetails.Id,
                            OrderId = orderResult.Result,
                            ItemId = plan.ItemId,
                            RequiresAction = true,
                        });

                        if (!createPlanResult.Success)
                        {
                            logger.Warn($"Failed to create plan '{plan.ItemId}' for order '{orderResult.Result}'.");
                        }
                    }
                }
            }

            return orderResult;
        }

        public async Task<bool> UpdateOrderStatusAsync(string transactionId, OrderStatus status)
        {
            return await repository.UpdateOrderStatusAsync(transactionId, status);
        }

        public async Task<OrderHistoryViewModel> GetOrderByOrderIdAsync(long orderId)
        {
            var order = await repository.GetOrderByOrderIdAsync(orderId);

            return mapper.Map<Order, OrderHistoryViewModel>(order);
        }

        public async Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(QuestionnaireViewModel questionnaireViewModel)
        {
            var questionnaire = mapper.Map<QuestionnaireViewModel, Questionnaire>(questionnaireViewModel);

            var result = await repository.CreateOrUpdateQuestionnaireAsync(questionnaire);
            if (!result.Success)
            {
                logger.Warn(string.Format(Resources.FailedToCreateOrUpdateQuestionnaireErrorMessage, questionnaireViewModel.OrderId));
                return result;
            }

            // associate the questionnaire to the order
            if (!await repository.AssociateQuestionnaireToOrderAsync(questionnaireViewModel.OrderId, result.Result))
            {
                logger.Warn(string.Format(Resources.FailedToAssociateQuestionnaireToPurchaseErrorMessage, questionnaireViewModel.OrderId));
                result.Success = false;
            }

            // associate the questionnaire to the order
            if (!await repository.AssociateQuestionnaireToPlansAsync(questionnaireViewModel.OrderId))
            {
                logger.Warn(string.Format(Resources.FailedToAssociateQuestionnaireToPlansErrorMessage, questionnaireViewModel.OrderId));
                result.Success = false;
            }

            return result;
        }

        public async Task<QuestionnaireViewModel> GetQuestionnaireAsync(long questionnaireId)
        {
            var questionnaire = await repository.GetQuestionnaireAsync(questionnaireId);

            return mapper.Map<Questionnaire, QuestionnaireViewModel>(questionnaire);
        }

        public async Task<bool> UpdatePlanStatusAsync(long planId, bool status)
        {
            return await repository.UpdatePlanStatusAsync(planId, status);
        }

        public async Task<bool> UpdateBlogStatusAsync(long blogId, bool status)
        {
            return await repository.UpdateBlogStatusAsync(blogId, status);
        }

        public async Task<AsyncResult<long>> CreateDiscountCodeAsync(CreateDiscountCodeViewModel discountCode)
        {
            var repoDiscountCode = mapper.Map<CreateDiscountCodeViewModel, DiscountCode>(discountCode);

            return await repository.CreateOrUpdateDiscountCodeAsync(repoDiscountCode);
        }

        public async Task<AsyncResult<long>> UpdateDiscountCodeAsync(DiscountCodeViewModel discountCode)
        {
            var repoDiscountCode = mapper.Map<DiscountCodeViewModel, DiscountCode>(discountCode);

            return await repository.CreateOrUpdateDiscountCodeAsync(repoDiscountCode);
        }

        public async Task<AsyncResult<long>> AddImageAsync(string imagePath)
        {
            var image = new Image() { ImagePath = imagePath };

            return await repository.AddImageAsync(image);
        }

        public async Task<ImageListViewModel> GetImagesAsync()
        {
            var images = await repository.GetImagesAsync();

            var imageListViewModel = new ImageListViewModel();

            if (images == null || !images.Any())
                imageListViewModel.Images = Enumerable.Empty<ImageViewModel>();

            imageListViewModel.Images = mapper.MapEnumerable<Image, ImageViewModel>(images);

            return imageListViewModel;
        }

        public async Task<AsyncResult<long>> CreateOrUpdateImageConfigurationAsync(ImageConfigurationViewModel imageConfiguration)
        {
            var repoImageConfiguration = mapper.Map<ImageConfigurationViewModel, ImageConfiguration>(imageConfiguration);

            return await repository.CreateOrUpdateImageConfigurationAsync(repoImageConfiguration);
        }

        public async Task<ImageConfigurationViewModel> GetImageConfigurationAsync()
        {
            var imageConfiguration = await repository.GetImageConfigurationAsync();

            var imageConfigurationViewModel = imageConfiguration != null
                                                ? mapper.Map<ImageConfiguration, ImageConfigurationViewModel>(imageConfiguration)
                                                : new ImageConfigurationViewModel() { Randomize = false };

            var images = await repository.GetImagesAsync();
            imageConfigurationViewModel.Images = images != null
                                                    ? mapper.MapEnumerable<Image, ImageViewModel>(images)
                                                    : Enumerable.Empty<ImageViewModel>();
            return imageConfigurationViewModel;
        }

        public async Task<SectionImageViewModel> GetSectionImagesAsync()
        {
            var imageConfiguration = await repository.GetImageConfigurationAsync();
            var images = await repository.GetImagesAsync();

            if (imageConfiguration == null || images == null || !images.Any())
            {
                return null;
            }

            imageConfiguration.Images = images;

            return mapper.Map<ImageConfiguration, SectionImageViewModel>(imageConfiguration);
        }

        public async Task<KaleidoscopeViewModel> GetKaleidoscopeImagesAsync()
        {
            var imageConfiguration = await repository.GetImageConfigurationAsync();
            var images = await repository.GetImagesAsync();

            if (imageConfiguration == null || images == null || !images.Any())
            {
                return null;
            }

            imageConfiguration.Images = images;

            return mapper.Map<ImageConfiguration, KaleidoscopeViewModel>(imageConfiguration);
        }

        public async Task<bool> UploadCustomerPlanAsync(long planId, string planPath)
        {
            return await repository.UploadCustomerPlanAsync(planId, planPath);
        }

        public async Task<bool> UploadHallOfFameAsync(long customerPlanId, string beforeImage, string afterImage, string comment)
        {
            bool success = false;

            var customerPlan = await repository.GetCustomerPlanAsync(customerPlanId);
            if (customerPlan != null)
            {
                customerPlan.BeforeImage = beforeImage;
                customerPlan.AfterImage = afterImage;
                customerPlan.Comment = comment;
                customerPlan.MemberOfHallOfFame = true;
                customerPlan.HallOfFameDate = DateTime.UtcNow;
                customerPlan.HallOfFameEnabled = false;

                success = await repository.UpdateHallOfFameDetailsAsync(customerPlan);
            }

            return success;
        }

        public async Task<IEnumerable<HallOfFameViewModel>> GetHallOfFameEntriesAsync(bool onlyEnabled = true, int? numberOfEntries = null)
        {
            var hallOfFameEntries = await repository.GetHallOfFameEntriesAsync(onlyEnabled, numberOfEntries);

            if (hallOfFameEntries == null || !hallOfFameEntries.Any())
                return Enumerable.Empty<HallOfFameViewModel>();

            var mappedHallOfFameEntriesViewModel = mapper.MapEnumerable<CustomerPlan, HallOfFameViewModel>(hallOfFameEntries);

            return mappedHallOfFameEntriesViewModel;
        }

        public async Task<bool> UpdateHallOfFameStatusAsync(long orderItemId, bool status)
        {
            return await repository.UpdateHallOfFameStatusAsync(orderItemId, status);
        }

        public async Task<bool> DeleteHallOfFameEntryAsync(long orderItemId)
        {
            return await repository.DeleteHallOfFameEntryAsync(orderItemId);
        }

        public Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers)
        {
            return emailService.SendEmailAsync(subject, content, receivers);
        }

        public Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers, IEnumerable<string> attachmentPaths)
        {
            return emailService.SendEmailAsync(subject, content, receivers, attachmentPaths);
        }

        public async Task<AsyncResult<long>> CreateMessageAsync(CreateMessageViewModel message)
        {
            var repoMessage = mapper.Map<CreateMessageViewModel, Message>(message);

            return await repository.CreateOrUpdateMessageAsync(repoMessage);
        }

        public async Task<AsyncResult<long>> UpdateMessageAsync(MessageViewModel message)
        {
            var repoMessage = mapper.Map<MessageViewModel, Message>(message);

            return await repository.CreateOrUpdateMessageAsync(repoMessage);
        }

        public async Task<IEnumerable<MessageViewModel>> GetMessagesAsync()
        {
            var messages = await repository.GetMessagesAsync();

            if (messages == null || !messages.Any())
                return Enumerable.Empty<MessageViewModel>();

            return mapper.MapEnumerable<Message, MessageViewModel>(messages);
        }

        public async Task<MessageViewModel> GetMessageAsync(long id)
        {
            var message = await repository.GetMessageAsync(id);

            if (message == null)
                return null;

            return mapper.Map<Message, MessageViewModel>(message);
        }

        public async Task<bool> DeleteImageAsync(long imageId)
        {
            return await repository.DeleteImageAsync(imageId);
        }

        private BasketViewModel ApplyDiscountCode(BasketViewModel basket)
        {
            basket.Items.Values.ToList().ForEach(i =>
            {
                i.ItemDiscounted = false;
            });

            // apply discount
            if (basket.DiscountCode != null)
            {
                var percentDiscount = basket.DiscountCode.PercentDiscount;
                basket.Items.Values.ToList().ForEach(i =>
                {
                    // apply the discount to all items
                    i.DiscountPercent = percentDiscount;
                    i.ItemDiscounted = true;
                });
            }

            return basket;
        }

        public async Task<AsyncResult<long>> CreateCustomerPlanAsync(CreateCustomerPlanViewModel customerPlan)
        {
            var plan = mapper.Map<CreateCustomerPlanViewModel, CustomerPlan>(customerPlan);

            return await repository.CreateCustomerPlanAsync(plan);
        }

        public async Task<AsyncResult<long>> UpdateCustomerPlanAsync(CustomerPlanViewModel customerPlan)
        {
            var plan = mapper.Map<CustomerPlanViewModel, CustomerPlan>(customerPlan);

            return await repository.UpdateCustomerPlanAsync(plan);
        }

        public async Task<IEnumerable<CustomerPlanViewModel>> GetCustomerPlansForOrderAsync(long orderId)
        {
            var plans = await repository.GetCustomerPlansForOrderAsync(orderId);

            return mapper.MapEnumerable<CustomerPlan, CustomerPlanViewModel>(plans);
        }

        public async Task<bool> MarkOrderCompleteAsync(long orderId)
        {
            return await repository.MarkOrderCompleteAsync(orderId);
        }
    }
}
