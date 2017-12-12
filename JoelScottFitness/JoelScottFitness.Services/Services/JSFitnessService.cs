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
        private readonly IEmailService emailService;

        public JSFitnessService(IJSFitnessRepository repository,
                                [Named("ServiceMapper")] IMapper  mapper, 
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

            var purchaseViewModel = mapper.Map<Purchase, PurchaseHistoryViewModel>(purchase);

            // map these objects like this to avoid circular mapper dependency
            if (purchase.Customer != null)
            {
                purchaseViewModel.Customer = mapper.Map<Customer, CustomerViewModel>(purchase.Customer);
            }
            
            if (purchase.DiscountCode != null)
            {
                purchaseViewModel.DiscountCode = mapper.Map<DiscountCode, DiscountCodeViewModel>(purchase.DiscountCode);
            }

            if (purchase.Questionnaire != null)
            {
                purchaseViewModel.Questionnaire = mapper.Map<Questionnaire, QuestionnaireViewModel>(purchase.Questionnaire);
            }

            var plans = await repository.GetPlansAsync();

            purchaseViewModel.Items.ToList().ForEach(pvm => 
            {
                var plan = plans.Where(p => p.Options.Select(s => s.Id).Contains(pvm.ItemId)).FirstOrDefault();
                pvm.Name = plan?.Name;
                pvm.ImagePath = plan?.ImagePathLarge;
            });
            
            return purchaseViewModel;
        }

        public async Task<IEnumerable<PurchaseSummaryViewModel>> GetPurchaseSummaryAsync(long customerId)
        {
            var purchases = await repository.GetPurchasesAsync(customerId);

            if (purchases == null || !purchases.Any())
                return Enumerable.Empty<PurchaseSummaryViewModel>();

            return mapper.MapEnumerable<Purchase, PurchaseSummaryViewModel>(purchases);
        }

        public async Task<IEnumerable<PurchaseSummaryViewModel>> GetPurchasesAsync()
        {
            var purchases = await repository.GetPurchasesAsync();

            if (purchases == null || !purchases.Any())
                return Enumerable.Empty<PurchaseSummaryViewModel>();

            return mapper.MapEnumerable<Purchase, PurchaseSummaryViewModel>(purchases);
        }

        public async Task<IEnumerable<PurchasedHistoryItemViewModel>> GetCustomerPlansAsync(long customerId)
        {
            List<PurchasedHistoryItemViewModel> plansViewModel = new List<PurchasedHistoryItemViewModel>();
            var planOptions = await repository.GetPlanOptionsAsync();
            var purchases = await repository.GetPurchasesAsync(customerId);

            foreach (var purchase in purchases)
            {
                var mappedPlans = mapper.MapEnumerable<PurchasedItem, PurchasedHistoryItemViewModel>(purchase.Items);
                mappedPlans.ToList().ForEach(p => 
                {
                    var plan = planOptions.FirstOrDefault(o => o.Id == p.PlanOptionId)?.Plan;
                    p.QuestionnaireComplete = purchase.QuestionnareId.HasValue;
                    p.TransactionId = purchase.TransactionId;
                    p.Name = plan.Name;
                    p.ImagePath = plan.ImagePathLarge;
                });

                plansViewModel.AddRange(mappedPlans);
            }
            
            return plansViewModel;
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

        public async Task<AsyncResult<long>> AddImage(string imagePath)
        {
            var image = new Image() { ImagePath = imagePath };

            return await repository.AddImage(image);
        }

        public async Task<ImageListViewModel> GetImages()
        {
            var images = await repository.GetImages();

            var imageListViewModel = new ImageListViewModel();

            if (images == null || !images.Any())
                imageListViewModel.Images = Enumerable.Empty<ImageViewModel>();

            imageListViewModel.Images = mapper.MapEnumerable<Image, ImageViewModel>(images);

            return imageListViewModel;
        }

        public async Task<AsyncResult<long>> CreateOrUpdateImageConfiguration(ImageConfigurationViewModel imageConfiguration)
        {
            var repoImageConfiguration = mapper.Map<ImageConfigurationViewModel, ImageConfiguration>(imageConfiguration);

            return await repository.CreateOrUpdateImageConfiguration(repoImageConfiguration);
        }

        public async Task<ImageConfigurationViewModel> GetImageConfiguration()
        {
            var imageConfiguration = await repository.GetImageConfiguration();

            var images = await repository.GetImages();
            
            var imageConfigurationViewModel = imageConfiguration != null 
                                                ? mapper.Map<ImageConfiguration, ImageConfigurationViewModel>(imageConfiguration) 
                                                : new ImageConfigurationViewModel();

            if (images != null)
            {
                imageConfigurationViewModel.Images = mapper.MapEnumerable<Image, ImageViewModel>(images);
            }

            return imageConfigurationViewModel;
        }

        public async Task<SectionImageViewModel> GetSectionImages()
        {
            var imageConfiguration = await repository.GetImageConfiguration();

            var images = await repository.GetImages();

            if (imageConfiguration != null)
            {
                imageConfiguration.Images = images != null 
                                                ? images 
                                                : Enumerable.Empty<Image>();
            }

            return mapper.Map<ImageConfiguration, SectionImageViewModel>(imageConfiguration);
        }

        public async Task<KaleidoscopeViewModel> GetKaleidoscopeImages()
        {
            var imageConfiguration = await repository.GetImageConfiguration();

            var images = await repository.GetImages();

            if (imageConfiguration != null)
            {
                imageConfiguration.Images = images != null
                                                ? images
                                                : Enumerable.Empty<Image>();
            }

            return mapper.Map<ImageConfiguration, KaleidoscopeViewModel>(imageConfiguration);
        }

        public async Task<bool> AssociatePlanToPurchase(long purchasedItemId, string planPath)
        {
            return await repository.AssociatePlanToPurchase(purchasedItemId, planPath);
        }

        public async Task<bool> UploadHallOfFameAsync(long purchasedItemId, string beforeImage, string afterImage, string comment)
        {
            bool success = false;

            var purchasedItem = await repository.GetPurchasedItemAsync(purchasedItemId);
            if (purchasedItem != null)
            {
                purchasedItem.BeforeImage = beforeImage;
                purchasedItem.AfterImage = afterImage;
                purchasedItem.Comment = comment;
                purchasedItem.MemberOfHallOfFame = true;

                success = await repository.UpdatePurchasedItemAsync(purchasedItem);
            }

            return success;
        }

        public async Task<IEnumerable<HallOfFameViewModel>> GetHallOfFameEntries(bool onlyEnabled = true)
        {
            var purchasedItems = await repository.GetHallOfFameEntriesAsync(onlyEnabled);
            var plans = await repository.GetPlansAsync();

            var mappedHallOfFameEntriesViewModel = mapper.MapEnumerable<PurchasedItem, HallOfFameViewModel>(purchasedItems);

            mappedHallOfFameEntriesViewModel.ToList().ForEach(p =>
            {
                p.PlanName = plans.Where(plan => plan.Options.Select(s => s.Id).Contains(p.ItemId)).FirstOrDefault().Name;
            });

            return mappedHallOfFameEntriesViewModel;
        }

        public async Task<bool> UpdateHallOfFameStatusAsync(long purchasedItemId, bool status)
        {
            return await repository.UpdateHallOfFameStatusAsync(purchasedItemId, status);
        }

        public async Task<bool> DeleteHallOfFameEntryAsync(long purchasedItemId)
        {
            return await repository.DeleteHallOfFameEntryAsync(purchasedItemId);
        }

        public Task<bool> SendEmail(string subject, string content, IEnumerable<string> receivers)
        {
            return emailService.SendEmailAsync(subject, content, receivers);
        }
    }
}
