using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data;
using JoelScottFitness.Data.Enumerations;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity.Models;
using JoelScottFitness.PayPal.Services;
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

        public JSFitnessService(IJSFitnessRepository repository,
                                [Named("ServiceMapper")] IMapper  mapper, 
                                IPayPalService paypalService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.paypalService = paypalService ?? throw new ArgumentNullException(nameof(paypalService));
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


        public async Task<AsyncResult<long>> CreateCustomerAsync(CreateCustomerViewModel customer)
        {
            var repoCustomer = mapper.Map<CreateCustomerViewModel, Customer>(customer);

            return await repository.CreateCustomerAsync(repoCustomer);
        }

        public async Task<AsyncResult<long>> UpdateCustomerAsync(CustomerViewModel customer)
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

        public async Task<CustomerViewModel> GetCustomerDetailsAsync(long id)
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

        public async Task<PurchaseHistoryViewModel> GetPurchaseAsync(long id)
        {
            var purchase = await repository.GetPurchaseAsync(id);

            if (purchase == null)
                return null;

            return mapper.Map<Purchase, PurchaseHistoryViewModel>(purchase);
        }

        public async Task<IEnumerable<PurchaseHistoryViewModel>> GetPurchasesAsync(long customerId)
        {
            var purchases = await repository.GetPurchasesAsync(customerId);

            if (purchases == null || !purchases.Any())
                return Enumerable.Empty<PurchaseHistoryViewModel>();

            return mapper.MapEnumerable<Purchase, PurchaseHistoryViewModel>(purchases);
        }

        public async Task<IEnumerable<PurchaseSummaryViewModel>> GetPurchasesAsync()
        {
            var purchases = await repository.GetPurchasesAsync();

            if (purchases == null || !purchases.Any())
                return Enumerable.Empty<PurchaseSummaryViewModel>();

            return mapper.MapEnumerable<Purchase, PurchaseSummaryViewModel>(purchases);
        }

        public PaymentInitiationResult InitiatePayPalPayment(ConfirmPurchaseViewModel confirmPurchaseViewModel, string baseUri)
        {
            return paypalService.InitiatePayPalPayment(confirmPurchaseViewModel, baseUri);
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
        public async Task<IEnumerable<SelectedPlanOptionViewModel>> GetBasketItemsAsync(IEnumerable<long> ids)
        {
            var repoPlanOptions = await repository.GetBasketItemsAsync(ids);

            return mapper.MapEnumerable<PlanOption, SelectedPlanOptionViewModel>(repoPlanOptions);
        }

        public async Task<UserViewModel> GetUserAsync(string userName)
        {
            var user = await repository.GetUserAsync(userName);

            return mapper.Map<AuthUser, UserViewModel>(user);
        }

        public async Task<AsyncResult<long>> SavePurchaseAsync(ConfirmPurchaseViewModel confirmPurchaseViewModel)
        {
            var purchase = mapper.Map<ConfirmPurchaseViewModel, Purchase>(confirmPurchaseViewModel);

            return await repository.SavePurchaseAsync(purchase);
        }

        public async Task<bool> UpdatePurchaseStatusAsync(string transactionId, PurchaseStatus status)
        {
            return await repository.UpdatePurchaseStatus(transactionId, status);
        }

        public async Task<PurchaseHistoryViewModel> GetPurchaseByTransactionIdAsync(string transactionId)
        {
            var purchase = await repository.GetPurchaseByTransactionId(transactionId);
            
            return mapper.Map<Purchase, PurchaseHistoryViewModel>(purchase);
        }

        public async Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(QuestionnaireViewModel questionnaireViewModel)
        {
            var questionnaire = mapper.Map<QuestionnaireViewModel, Questionnaire>(questionnaireViewModel);

            var result = await repository.CreateOrUpdateQuestionnaireAsync(questionnaire);

            // associate the questionnaire to the purchase
            await repository.AssociateQuestionnaireToPurchase(questionnaireViewModel.PurchaseId, result.Result);

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
    }
}
