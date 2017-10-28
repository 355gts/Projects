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
        
        public async Task<AsyncResult<long>> CreateOrUpdateBlog(BlogViewModel blog)
        {
            var repoBlog = mapper.Map<BlogViewModel, Blog>(blog);

            return await repository.CreateOrUpdateBlogAsync(repoBlog);
        }

        public async Task<AsyncResult<long>> CreateCustomer(CreateCustomerViewModel customer)
        {
            var repoCustomer = mapper.Map<CreateCustomerViewModel, Customer>(customer);

            return await repository.CreateCustomerAsync(repoCustomer);
        }

        public async Task<AsyncResult<long>> UpdateCustomer(CustomerViewModel customer)
        {
            var repoCustomer = mapper.Map<CustomerViewModel, Customer>(customer);

            return await repository.UpdateCustomerAsync(repoCustomer);
        }

        public async Task<AsyncResult<long>> CreateOrUpdateDiscountCode(DiscountCodeViewModel discountCode)
        {
            var repoDiscountCode = mapper.Map<DiscountCodeViewModel, DiscountCode>(discountCode);

            return await repository.CreateOrUpdateDiscountCodeAsync(repoDiscountCode);
        }

        public async Task<AsyncResult<long>> CreateOrUpdatePlan(PlanViewModel plan)
        {
            var repoPlan = mapper.Map<PlanViewModel, Plan>(plan);

            return await repository.CreateOrUpdatePlanAsync(repoPlan);
        }

        public async Task<bool> DeactivateBlog(long id)
        {
            return await repository.DeactivateBlogAsync(id);
        }

        public async Task<bool> DeactivatePlan(long id)
        {
            return await repository.DeactivatePlanAsync(id);
        }

        public async Task<BlogViewModel> GetBlog(long id)
        {
            var blog = await repository.GetBlogAsync(id);

            if (blog == null)
                return null;

            return mapper.Map<Blog, BlogViewModel>(blog);
        }

        public async Task<IEnumerable<BlogViewModel>> GetBlogs(int number = 0, bool activeOnly = true)
        {
            var blogs = await repository.GetBlogsAsync(number, activeOnly);

            if (blogs == null || !blogs.Any())
                return Enumerable.Empty<BlogViewModel>();

            return mapper.MapEnumerable<Blog, BlogViewModel>(blogs);
        }

        public async Task<CustomerViewModel> GetCustomerDetails(long id)
        {
            var customer = await repository.GetCustomerDetailsAsync(id);

            if (customer == null)
                return null;

            return mapper.Map<Customer, CustomerViewModel>(customer);
        }

        public async Task<CustomerViewModel> GetCustomerDetails(string userName)
        {
            var customer = await repository.GetCustomerDetailsAsync(userName);

            if (customer == null)
                return null;

            return mapper.Map<Customer, CustomerViewModel>(customer);
        }

        public async Task<DiscountCodeViewModel> GetDiscountCode(long id)
        {
            var discountCode = await repository.GetDiscountCodeAsync(id);

            if (discountCode == null)
                return null;

            return mapper.Map<DiscountCode, DiscountCodeViewModel>(discountCode);
        }

        public async Task<IEnumerable<DiscountCodeViewModel>> GetDiscountCodes()
        {
            var discountCodes = await repository.GetDiscountCodesAsync();

            if (discountCodes == null || !discountCodes.Any())
                return Enumerable.Empty<DiscountCodeViewModel>();

            return mapper.MapEnumerable<DiscountCode, DiscountCodeViewModel>(discountCodes);
        }

        public async Task<PlanViewModel> GetPlan(long id)
        {
            var plan = await repository.GetPlanAsync(id);

            if (plan == null)
                return null;

            return mapper.Map<Plan, PlanViewModel>(plan);
        }

        public async Task<IEnumerable<PlanViewModel>> GetPlans()
        {
            var plans = await repository.GetPlansAsync();

            if (plans == null || !plans.Any())
                return Enumerable.Empty<PlanViewModel>();

            return mapper.MapEnumerable<Plan, PlanViewModel>(plans);
        }

        public async Task<IEnumerable<PlanViewModel>> GetPlansByGender(Gender gender)
        {
            var plans = await repository.GetPlansByGenderAsync(gender);

            if (plans == null || !plans.Any())
                return Enumerable.Empty<PlanViewModel>();

            return mapper.MapEnumerable<Plan, PlanViewModel>(plans);
        }

        public async Task<PlanOptionViewModel> GetPlanOptionAsync(long id)
        {
            var planOption = await repository.GetPlanOptionAsync(id);

            if (planOption == null)
                return null;

            return mapper.Map<PlanOption, PlanOptionViewModel>(planOption);
        }

        public async Task<PurchaseHistoryViewModel> GetPurchase(long id)
        {
            var purchase = await repository.GetPurchaseAsync(id);

            if (purchase == null)
                return null;

            return mapper.Map<Purchase, PurchaseHistoryViewModel>(purchase);
        }

        public async Task<IEnumerable<PurchaseHistoryViewModel>> GetPurchases(long customerId)
        {
            var purchases = await repository.GetPurchasesAsync(customerId);

            if (purchases == null || !purchases.Any())
                return Enumerable.Empty<PurchaseHistoryViewModel>();

            return mapper.MapEnumerable<Purchase, PurchaseHistoryViewModel>(purchases);
        }

        public PaymentInitiationResult InitiatePayPalPayment(ConfirmPurchaseViewModel confirmPurchaseViewModel, string baseUri)
        {
            return paypalService.InitiatePayPalPayment(confirmPurchaseViewModel, baseUri);
        }


        public PaymentResult CompletePayPalPayment(string paymentId, string payerId)
        {
            return paypalService.CompletePayPalPayment(paymentId, payerId);
        }

        public async Task<bool> UpdateMailingList(MailingListItemViewModel mailingListItem)
        {
            var repoMailingListItem = mapper.Map<MailingListItemViewModel, MailingListItem>(mailingListItem);

            return await repository.UpdateMailingListAsync(repoMailingListItem);
        }
        public async Task<IEnumerable<PlanOptionViewModel>> GetBasketItems(IEnumerable<long> ids)
        {
            var repoPlanOptions = await repository.GetBasketItemsAsync(ids);

            return mapper.MapEnumerable<PlanOption, PlanOptionViewModel>(repoPlanOptions);
        }

        public async Task<UserViewModel> GetUser(string userName)
        {
            var user = await repository.GetUserAsync(userName);

            return mapper.Map<AuthUser, UserViewModel>(user);
        }

        public async Task<AsyncResult<long>> SavePurchase(ConfirmPurchaseViewModel confirmPurchaseViewModel)
        {
            var purchase = mapper.Map<ConfirmPurchaseViewModel, Purchase>(confirmPurchaseViewModel);

            return await repository.SavePurchaseAsync(purchase);
        }

        public async Task<bool> UpdatePurchaseStatus(string transactionId, PurchaseStatus status)
        {
            return await repository.UpdatePurchaseStatus(transactionId, status);
        }

        public async Task<long?> GetPurchaseIdByTransactionId(string transactionId)
        {
            return await repository.GetPurchaseIdByTransactionId(transactionId);
        }
    }
}
