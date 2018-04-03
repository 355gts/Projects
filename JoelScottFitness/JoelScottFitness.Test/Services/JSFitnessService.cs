using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity.Models;
using JoelScottFitness.PayPal.Services;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using SRV = JoelScottFitness.Services.Services;

namespace JoelScottFitness.Test.Services
{
    [TestClass]
    public partial class JSFitnessService
    {
        Mock<IJSFitnessRepository> repositoryMock;
        Mock<IPayPalService> paypalServiceMock;
        Mock<IEmailService> emailServiceMock;

        string userName = "userName";
        Guid customerId = Guid.NewGuid();
        string idString = "id";
        long id = 1234;
        OrderItem purchasedItemCallback;
        CustomerPlan customerPlanCallback;

        SRV.JSFitnessService service;

        [TestInitialize]
        public void TestSetup()
        {
            repositoryMock = new Mock<IJSFitnessRepository>();
            paypalServiceMock = new Mock<IPayPalService>();
            emailServiceMock = new Mock<IEmailService>();

            repositoryMock.Setup(r => r.CreateOrUpdateBlogAsync(It.IsAny<Blog>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true, Result = id });
            repositoryMock.Setup(r => r.CreateCustomerAsync(It.IsAny<Customer>()))
                          .ReturnsAsync(new AsyncResult<Guid>() { Success = true, Result = Guid.NewGuid() });
            repositoryMock.Setup(r => r.UpdateCustomerAsync(It.IsAny<Customer>()))
                          .ReturnsAsync(new AsyncResult<Guid>() { Success = true, Result = Guid.NewGuid() });
            repositoryMock.Setup(r => r.CreateOrUpdatePlanAsync(It.IsAny<Plan>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true, Result = id });
            repositoryMock.Setup(r => r.GetBlogAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Blog());
            repositoryMock.Setup(r => r.GetBlogsAsync(It.IsAny<int>()))
                          .ReturnsAsync(new List<Blog>() { new Blog(), new Blog() });
            repositoryMock.Setup(r => r.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                          .ReturnsAsync(new Customer() { Id = customerId });
            repositoryMock.Setup(r => r.GetCustomerDetailsAsync(It.IsAny<string>()))
                          .ReturnsAsync(new Customer() { Id = customerId });
            repositoryMock.Setup(r => r.GetDiscountCodeAsync(It.IsAny<long>()))
                          .ReturnsAsync(new DiscountCode());
            repositoryMock.Setup(r => r.GetDiscountCodeAsync(It.IsAny<string>()))
                          .ReturnsAsync(new DiscountCode());
            repositoryMock.Setup(r => r.GetDiscountCodesAsync())
                          .ReturnsAsync(new List<DiscountCode>() { new DiscountCode(), new DiscountCode() });
            repositoryMock.Setup(r => r.GetPlanAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Plan());
            repositoryMock.Setup(r => r.GetPlansAsync())
                          .ReturnsAsync(new List<Plan>() { new Plan(), new Plan() });
            repositoryMock.Setup(r => r.GetPlansByGenderAsync(It.IsAny<Gender>()))
                          .ReturnsAsync(new List<Plan>() { new Plan(), new Plan() });
            repositoryMock.Setup(r => r.GetPlanOptionAsync(It.IsAny<long>()))
                          .ReturnsAsync(new PlanOption());
            repositoryMock.Setup(r => r.UpdateMailingListAsync(It.IsAny<MailingListItem>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()))
                          .ReturnsAsync(new List<PlanOption>() { new PlanOption(), new PlanOption() });
            repositoryMock.Setup(r => r.SavePurchaseAsync(It.IsAny<Order>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true });
            repositoryMock.Setup(r => r.UpdatePurchaseStatusAsync(It.IsAny<string>(), It.IsAny<PurchaseStatus>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetPurchaseByOrderIdAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Order());
            repositoryMock.Setup(r => r.CreateOrUpdateQuestionnaireAsync(It.IsAny<Questionnaire>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true });
            repositoryMock.Setup(r => r.AssociateQuestionnaireToPurchaseAsync(It.IsAny<long>(), It.IsAny<long>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.AssociateQuestionnaireToPlansAsync(It.IsAny<long>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetQuestionnaireAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Questionnaire());
            repositoryMock.Setup(r => r.UpdatePlanStatusAsync(It.IsAny<long>(), It.IsAny<bool>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.CreateOrUpdateDiscountCodeAsync(It.IsAny<DiscountCode>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true });
            repositoryMock.Setup(r => r.AddImageAsync(It.IsAny<Image>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true });
            repositoryMock.Setup(r => r.GetImagesAsync())
                          .ReturnsAsync(new List<Image>() { new Image(), new Image() });
            repositoryMock.Setup(r => r.CreateOrUpdateImageConfigurationAsync(It.IsAny<ImageConfiguration>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true });
            repositoryMock.Setup(r => r.GetImageConfigurationAsync())
                          .ReturnsAsync(new ImageConfiguration());
            repositoryMock.Setup(r => r.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetPurchasedItemAsync(It.IsAny<long>()))
                          .ReturnsAsync(new OrderItem());
            repositoryMock.Setup(r => r.UpdatePurchasedItemAsync(It.IsAny<OrderItem>()))
                          .Callback<OrderItem>((a) => { purchasedItemCallback = a; })
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.UpdateHallOfFameStatusAsync(It.IsAny<long>(), It.IsAny<bool>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.DeleteHallOfFameEntryAsync(It.IsAny<long>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetUserAsync(It.IsAny<string>()))
                          .ReturnsAsync(new AuthUser());
            repositoryMock.Setup(r => r.GetPurchasesAsync(It.IsAny<Guid>()))
                          .ReturnsAsync(new List<Order>() { new Order(), new Order() });
            repositoryMock.Setup(r => r.CreateOrUpdateMessageAsync(It.IsAny<Message>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true });
            repositoryMock.Setup(r => r.GetMessageAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Message());
            repositoryMock.Setup(r => r.GetMessagesAsync())
                          .ReturnsAsync(new List<Message>() { new Message(), new Message() });
            repositoryMock.Setup(r => r.UpdateHallOfFameDetailsAsync(It.IsAny<CustomerPlan>()))
                          .Callback<CustomerPlan>((a) => { customerPlanCallback = a; })
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetHallOfFameEntriesAsync(It.IsAny<bool>(), It.IsAny<int?>()))
                          .ReturnsAsync(new List<CustomerPlan>() { new CustomerPlan() });

            paypalServiceMock.Setup(r => r.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(new PaymentResult() { Success = true });
            paypalServiceMock.Setup(r => r.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()))
                             .Returns(new PaymentInitiationResult() { Success = true });


            emailServiceMock.Setup(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                            .ReturnsAsync(true);
            emailServiceMock.Setup(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
                            .ReturnsAsync(true);

            service = new SRV.JSFitnessService(repositoryMock.Object,
                                     MapperHelper.GetServiceMapper(),
                                     paypalServiceMock.Object,
                                     emailServiceMock.Object);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RepositoryNull_ThrowsArgumentNullException()
        {
            new SRV.JSFitnessService(null,
                                     new Mock<IMapper>().Object,
                                     new Mock<IPayPalService>().Object,
                                     new Mock<IEmailService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_MapperNull_ThrowsArgumentNullException()
        {
            new SRV.JSFitnessService(new Mock<IJSFitnessRepository>().Object,
                                     null,
                                     new Mock<IPayPalService>().Object,
                                     new Mock<IEmailService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PayPalServiceNull_ThrowsArgumentNullException()
        {
            new SRV.JSFitnessService(new Mock<IJSFitnessRepository>().Object,
                                     new Mock<IMapper>().Object,
                                     null,
                                     new Mock<IEmailService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_EmailServiceNull_ThrowsArgumentNullException()
        {
            new SRV.JSFitnessService(new Mock<IJSFitnessRepository>().Object,
                                     new Mock<IMapper>().Object,
                                     new Mock<IPayPalService>().Object,
                                     null);
        }
        #endregion

        [TestMethod]
        public void CreateBlogAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdateBlogAsync(It.IsAny<Blog>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });
            // test
            var result = service.CreateBlogAsync(new CreateBlogViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateBlogAsync(It.IsAny<Blog>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateBlogAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.CreateBlogAsync(new CreateBlogViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateBlogAsync(It.IsAny<Blog>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(id, result.Result);
        }

        [TestMethod]
        public void UpdateBlogAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdateBlogAsync(It.IsAny<Blog>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });
            // test
            var result = service.UpdateBlogAsync(new BlogViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateBlogAsync(It.IsAny<Blog>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void UpdateBlogAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UpdateBlogAsync(new BlogViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateBlogAsync(It.IsAny<Blog>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(id, result.Result);
        }

        [TestMethod]
        public void CreateCustomerAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateCustomerAsync(It.IsAny<Customer>()))
                          .ReturnsAsync(new AsyncResult<Guid>() { Success = false });

            // test
            var result = service.CreateCustomerAsync(new CreateCustomerViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateCustomerAsync(It.IsAny<Customer>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateCustomerAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.CreateCustomerAsync(new CreateCustomerViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateCustomerAsync(It.IsAny<Customer>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(typeof(Guid), result.Result.GetType());
        }

        [TestMethod]
        public void UpdateCustomerAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.UpdateCustomerAsync(It.IsAny<Customer>()))
                          .ReturnsAsync(new AsyncResult<Guid>() { Success = false });

            // test
            var result = service.UpdateCustomerAsync(new CustomerViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void UpdateCustomerAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UpdateCustomerAsync(new CustomerViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(typeof(Guid), result.Result.GetType());
        }

        [TestMethod]
        public void CreatePlanAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdatePlanAsync(It.IsAny<Plan>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });

            // test
            var result = service.CreatePlanAsync(new CreatePlanViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdatePlanAsync(It.IsAny<Plan>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreatePlanAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.CreatePlanAsync(new CreatePlanViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdatePlanAsync(It.IsAny<Plan>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(id, result.Result);
        }

        [TestMethod]
        public void UpdatePlanAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdatePlanAsync(It.IsAny<Plan>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });

            // test
            var result = service.UpdatePlanAsync(new PlanViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdatePlanAsync(It.IsAny<Plan>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void UpdatePlanAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UpdatePlanAsync(new PlanViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdatePlanAsync(It.IsAny<Plan>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(id, result.Result);
        }

        [TestMethod]
        public void GetBlogAsync_Fails_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetBlogAsync(It.IsAny<long>()))
                          .ReturnsAsync((Blog)null);
            // test
            var result = service.GetBlogAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetBlogAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetBlogAsync_Success_ReturnsBlog()
        {
            // test
            var result = service.GetBlogAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetBlogAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetBlogsAsync_Fails_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetBlogsAsync(It.IsAny<int>()))
                          .ReturnsAsync((IEnumerable<Blog>)null);

            // test
            var result = service.GetBlogsAsync(5).Result;

            // verify
            repositoryMock.Verify(r => r.GetBlogsAsync(It.IsAny<int>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetBlogsAsync_Success_ReturnsBlogs()
        {
            // test
            var result = service.GetBlogsAsync(5).Result;

            // verify
            repositoryMock.Verify(r => r.GetBlogsAsync(It.IsAny<int>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetCustomerDetailsAsync_CustomerNotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((Customer)null);

            // test
            var result = service.GetCustomerDetailsAsync(customerId).Result;

            // verify
            repositoryMock.Verify(r => r.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCustomerDetailsAsync_CustomerFound_ReturnsCustomer()
        {
            // test
            var result = service.GetCustomerDetailsAsync(customerId).Result;

            // verify
            repositoryMock.Verify(r => r.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCustomerDetailsAsync_ByUsername_CustomerNotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetCustomerDetailsAsync(It.IsAny<string>()))
                          .ReturnsAsync((Customer)null);

            // test
            var result = service.GetCustomerDetailsAsync(userName).Result;

            // verify
            repositoryMock.Verify(r => r.GetCustomerDetailsAsync(It.IsAny<string>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCustomerDetailsAsync_ByUsername_CustomerFound_ReturnsCustomer()
        {
            // test
            var result = service.GetCustomerDetailsAsync(userName).Result;

            // verify
            repositoryMock.Verify(r => r.GetCustomerDetailsAsync(It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDiscountCodeAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetDiscountCodeAsync(It.IsAny<long>()))
                          .ReturnsAsync((DiscountCode)null);
            // test
            var result = service.GetDiscountCodeAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetDiscountCodeAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDiscountCodeAsync_Found_ReturnsDiscountCode()
        {
            // test
            var result = service.GetDiscountCodeAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetDiscountCodeAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDiscountCodeAsync_ByCode_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetDiscountCodeAsync(It.IsAny<string>()))
                          .ReturnsAsync((DiscountCode)null);
            // test
            var result = service.GetDiscountCodeAsync(idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetDiscountCodeAsync(It.IsAny<string>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDiscountCodeAsync_ByCode_Found_ReturnsDiscountCode()
        {
            // test
            var result = service.GetDiscountCodeAsync(idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetDiscountCodeAsync(It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDiscountCodesAsync_Fails_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetDiscountCodesAsync())
                          .ReturnsAsync((IEnumerable<DiscountCode>)null);

            // test
            var result = service.GetDiscountCodesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetDiscountCodesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetDiscountCodesAsync_Success_ReturnsDiscountCodes()
        {
            // test
            var result = service.GetDiscountCodesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetDiscountCodesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPlanAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlanAsync(It.IsAny<long>()))
                          .ReturnsAsync((Plan)null);

            // test
            var result = service.GetPlanAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlanAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPlanAsync_Success_ReturnsPlan()
        {
            // test
            var result = service.GetPlanAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlanAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetPlansAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlansAsync())
                          .ReturnsAsync((IEnumerable<Plan>)null);

            // test
            var result = service.GetPlansAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetPlansAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPlansAsync_Success_ReturnsPlan()
        {
            // test
            var result = service.GetPlansAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetPlansAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPlansByGenderAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlansByGenderAsync(It.IsAny<Gender>()))
                          .ReturnsAsync((IEnumerable<Plan>)null);

            // test
            var result = service.GetPlansByGenderAsync(Gender.Female).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlansByGenderAsync(It.IsAny<Gender>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPlansByGenderAsync_Success_ReturnsPlan()
        {
            // test
            var result = service.GetPlansByGenderAsync(Gender.Female).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlansByGenderAsync(It.IsAny<Gender>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPlanOptionAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlanOptionAsync(It.IsAny<long>()))
                          .ReturnsAsync((PlanOption)null);

            // test
            var result = service.GetPlanOptionAsync(It.IsAny<long>()).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlanOptionAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPlanOptionAsync_Success_ReturnsPlanOption()
        {
            // test
            var result = service.GetPlanOptionAsync(It.IsAny<long>()).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlanOptionAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InitiatePayPalPayment_Fails_ReturnsFalse()
        {
            // setup
            paypalServiceMock.Setup(r => r.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()))
                             .Returns(new PaymentInitiationResult() { Success = false });

            // test
            var result = service.InitiatePayPalPayment(new ConfirmPurchaseViewModel(), "baseUri");

            // verify
            paypalServiceMock.Verify(r => r.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void InitiatePayPalPayment_Success_ReturnsTrue()
        {
            // test
            var result = service.InitiatePayPalPayment(new ConfirmPurchaseViewModel(), "baseUri");

            // verify
            paypalServiceMock.Verify(r => r.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void CompletePayPalPayment_Fails_ReturnsFalse()
        {
            // setup
            paypalServiceMock.Setup(r => r.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(new PaymentResult() { Success = false });

            // test
            var result = service.CompletePayPalPayment("paymentId", "payerId");

            // verify
            paypalServiceMock.Verify(r => r.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CompletePayPalPayment_Success_ReturnsTrue()
        {
            // test
            var result = service.CompletePayPalPayment("paymentId", "payerId");

            // verify
            paypalServiceMock.Verify(r => r.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void UpdateMailingListAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.UpdateMailingListAsync(It.IsAny<MailingListItem>()))
                          .ReturnsAsync(false);

            // test
            var result = service.UpdateMailingListAsync(new MailingListItemViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.UpdateMailingListAsync(It.IsAny<MailingListItem>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateMailingListAsync_Sucess_ReturnsTrue()
        {
            // test
            var result = service.UpdateMailingListAsync(new MailingListItemViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.UpdateMailingListAsync(It.IsAny<MailingListItem>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        //[TestMethod]
        //public void GetBasketItemsAsync_NotFound_ReturnsEmptyList()
        //{
        //    // setup
        //    repositoryMock.Setup(r => r.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()))
        //                  .ReturnsAsync((IEnumerable<PlanOption>)null);
        //    // test
        //    var result = service.GetBasketAsync(new List<long>() { id }).Result;

        //    // verify
        //    repositoryMock.Verify(r => r.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(0, result.SelectedOptions.Count());
        //}

        //[TestMethod]
        //public void GetBasketItemsAsync_Success_ReturnsBasketItems()
        //{
        //    // test
        //    var result = service.GetBasketAsync(new List<long>() { id }).Result;

        //    // verify
        //    repositoryMock.Verify(r => r.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(2, result.SelectedOptions.Count());
        //}

        [TestMethod]
        public void GetUserAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetUserAsync(It.IsAny<string>()))
                          .ReturnsAsync((AuthUser)null);

            // test
            var result = service.GetUserAsync(idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetUserAsync(It.IsAny<string>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUserAsync_Found_ReturnsUser()
        {
            // test
            var result = service.GetUserAsync(idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetUserAsync(It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SavePurchaseAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.SavePurchaseAsync(It.IsAny<Order>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });
            // test
            var result = service.SavePurchaseAsync(new ConfirmPurchaseViewModel()
            {
                CustomerDetails = new CustomerViewModel() { Id = Guid.NewGuid() },
                Basket = new BasketViewModel() { Items = new Dictionary<long, BasketItemViewModel>() { { 1, new BasketItemViewModel() { Price = 10 } } } },
            }).Result;

            // verify
            repositoryMock.Verify(r => r.SavePurchaseAsync(It.IsAny<Order>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void SavePurchaseAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.SavePurchaseAsync(new ConfirmPurchaseViewModel()
            {
                CustomerDetails = new CustomerViewModel() { Id = Guid.NewGuid() },
                Basket = new BasketViewModel()
                {
                    Items = new Dictionary<long, BasketItemViewModel>()
                    {
                        { 1, new BasketItemViewModel() { Price = 10 } },
                    }
                }
            }).Result;

            // verify
            repositoryMock.Verify(r => r.SavePurchaseAsync(It.IsAny<Order>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void UpdatePurchaseStatusAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.UpdatePurchaseStatusAsync(It.IsAny<string>(), It.IsAny<PurchaseStatus>()))
                          .ReturnsAsync(false);
            // test
            var result = service.UpdatePurchaseStatusAsync(idString, PurchaseStatus.Complete).Result;

            // verify
            repositoryMock.Verify(r => r.UpdatePurchaseStatusAsync(It.IsAny<string>(), It.IsAny<PurchaseStatus>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdatePurchaseStatusAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UpdatePurchaseStatusAsync(idString, PurchaseStatus.Complete).Result;

            // verify
            repositoryMock.Verify(r => r.UpdatePurchaseStatusAsync(It.IsAny<string>(), It.IsAny<PurchaseStatus>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetPurchaseByOrderIdAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetPurchaseByOrderIdAsync(It.IsAny<long>()))
                          .ReturnsAsync((Order)null);

            // test
            var result = service.GetPurchaseByOrderIdAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchaseByOrderIdAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPurchaseByOrderIdAsync_Success_ReturnsPurchase()
        {
            // test
            var result = service.GetPurchaseByOrderIdAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchaseByOrderIdAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateOrUpdateQuestionnaireAsync_Fails_DoesNotAssociate_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdateQuestionnaireAsync(It.IsAny<Questionnaire>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });
            // test
            var result = service.CreateOrUpdateQuestionnaireAsync(new QuestionnaireViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateQuestionnaireAsync(It.IsAny<Questionnaire>()), Times.Once);
            repositoryMock.Verify(r => r.AssociateQuestionnaireToPurchaseAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
            repositoryMock.Verify(r => r.AssociateQuestionnaireToPlansAsync(It.IsAny<long>()), Times.Never);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateOrUpdateQuestionnaireAsync_FailsToAssociateWithPurchase_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.AssociateQuestionnaireToPurchaseAsync(It.IsAny<long>(), It.IsAny<long>()))
                          .ReturnsAsync(false);
            // test
            var result = service.CreateOrUpdateQuestionnaireAsync(new QuestionnaireViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateQuestionnaireAsync(It.IsAny<Questionnaire>()), Times.Once);
            repositoryMock.Verify(r => r.AssociateQuestionnaireToPurchaseAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            repositoryMock.Verify(r => r.AssociateQuestionnaireToPlansAsync(It.IsAny<long>()), Times.Never);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateOrUpdateQuestionnaireAsync_FailsToAssociateWithPlans_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.AssociateQuestionnaireToPlansAsync(It.IsAny<long>()))
                          .ReturnsAsync(false);
            // test
            var result = service.CreateOrUpdateQuestionnaireAsync(new QuestionnaireViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateQuestionnaireAsync(It.IsAny<Questionnaire>()), Times.Once);
            repositoryMock.Verify(r => r.AssociateQuestionnaireToPurchaseAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            repositoryMock.Verify(r => r.AssociateQuestionnaireToPlansAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateOrUpdateQuestionnaireAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.CreateOrUpdateQuestionnaireAsync(new QuestionnaireViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateQuestionnaireAsync(It.IsAny<Questionnaire>()), Times.Once);
            repositoryMock.Verify(r => r.AssociateQuestionnaireToPurchaseAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            repositoryMock.Verify(r => r.AssociateQuestionnaireToPlansAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetQuestionnaireAsync_Fails_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetQuestionnaireAsync(It.IsAny<long>()))
                          .ReturnsAsync((Questionnaire)null);

            // test
            var result = service.GetQuestionnaireAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetQuestionnaireAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetQuestionnaireAsync_Success_ReturnsQuestionnaire()
        {
            // test
            var result = service.GetQuestionnaireAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetQuestionnaireAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdatePlanStatusAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.UpdatePlanStatusAsync(It.IsAny<long>(), It.IsAny<bool>()))
                          .ReturnsAsync(false);

            // test
            var result = service.UpdatePlanStatusAsync(id, true).Result;

            // verify
            repositoryMock.Verify(r => r.UpdatePlanStatusAsync(It.IsAny<long>(), It.IsAny<bool>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdatePlanStatusAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UpdatePlanStatusAsync(id, true).Result;

            // verify
            repositoryMock.Verify(r => r.UpdatePlanStatusAsync(It.IsAny<long>(), It.IsAny<bool>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateDiscountCodeAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdateDiscountCodeAsync(It.IsAny<DiscountCode>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });

            // test
            var result = service.CreateDiscountCodeAsync(new CreateDiscountCodeViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateDiscountCodeAsync(It.IsAny<DiscountCode>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateDiscountCodeAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.CreateDiscountCodeAsync(new CreateDiscountCodeViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateDiscountCodeAsync(It.IsAny<DiscountCode>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void UpdateDiscountCodeAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdateDiscountCodeAsync(It.IsAny<DiscountCode>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });

            // test
            var result = service.UpdateDiscountCodeAsync(new DiscountCodeViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateDiscountCodeAsync(It.IsAny<DiscountCode>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void UpdateDiscountCodeAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UpdateDiscountCodeAsync(new DiscountCodeViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateDiscountCodeAsync(It.IsAny<DiscountCode>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddImage_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.AddImageAsync(It.IsAny<Image>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });

            // test
            var result = service.AddImageAsync("image").Result;

            // verify
            repositoryMock.Verify(r => r.AddImageAsync(It.IsAny<Image>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void AddImage_Success_ReturnsTrue()
        {
            // test
            var result = service.AddImageAsync("image").Result;

            // verify
            repositoryMock.Verify(r => r.AddImageAsync(It.IsAny<Image>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetImagesAsync_Fails_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetImagesAsync())
                          .ReturnsAsync((IEnumerable<Image>)null);

            // test
            var result = service.GetImagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Images.Count());
        }

        [TestMethod]
        public void GetImagesAsync_Success_ReturnsImages()
        {
            // test
            var result = service.GetImagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Images.Count());
        }

        [TestMethod]
        public void CreateOrUpdateImageConfigurationAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdateImageConfigurationAsync(It.IsAny<ImageConfiguration>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });

            // test
            var result = service.CreateOrUpdateImageConfigurationAsync(new ImageConfigurationViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateImageConfigurationAsync(It.IsAny<ImageConfiguration>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateOrUpdateImageConfigurationAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.CreateOrUpdateImageConfigurationAsync(new ImageConfigurationViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateImageConfigurationAsync(It.IsAny<ImageConfiguration>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetImageConfigurationAsync_ImageConfigurationNull_ImagesNull_ReturnsEmptyModel()
        {
            // setup
            repositoryMock.Setup(r => r.GetImageConfigurationAsync()).ReturnsAsync((ImageConfiguration)null);
            repositoryMock.Setup(r => r.GetImagesAsync()).ReturnsAsync((IEnumerable<Image>)null);

            // test
            var result = service.GetImageConfigurationAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImageConfigurationAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Randomize);
            Assert.AreEqual(0, result.Images.Count());
        }

        [TestMethod]
        public void GetImageConfigurationAsync_Success_ReturnsEmptyImages()
        {
            // setup
            repositoryMock.Setup(r => r.GetImageConfigurationAsync()).ReturnsAsync(new ImageConfiguration() { Randomize = true });

            // test
            var result = service.GetImageConfigurationAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImageConfigurationAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Randomize);
            Assert.AreEqual(2, result.Images.Count());
        }

        [TestMethod]
        public void GetSectionImagesAsync_ImageConfigurationNull_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetImageConfigurationAsync()).ReturnsAsync((ImageConfiguration)null);

            // test
            var result = service.GetSectionImagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImageConfigurationAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSectionImagesAsync_ImagesNull_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetImagesAsync()).ReturnsAsync((IEnumerable<Image>)null);

            // test
            var result = service.GetSectionImagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImageConfigurationAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSectionImagesAsync_Success_ReturnsModel()
        {
            // setup
            repositoryMock.Setup(r => r.GetImageConfigurationAsync()).ReturnsAsync(new ImageConfiguration() { Randomize = true });
            repositoryMock.Setup(r => r.GetImagesAsync()).ReturnsAsync(new List<Image>()
            {
                new Image() { ImagePath = "Path1" },
                new Image() { ImagePath = "Path2" },
                new Image() { ImagePath = "Path3" },
                new Image() { ImagePath = "Path4" },
            });

            // test
            var result = service.GetSectionImagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImageConfigurationAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(!string.IsNullOrEmpty(result.SectionImage1));
            Assert.IsTrue(!string.IsNullOrEmpty(result.SectionImage2));
            Assert.IsTrue(!string.IsNullOrEmpty(result.SectionImage3));
            Assert.IsTrue(!string.IsNullOrEmpty(result.SplashImage));
        }

        [TestMethod]
        public void GetKaleidoscopeImagesAsync_ImageConfigurationNull_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetImageConfigurationAsync()).ReturnsAsync((ImageConfiguration)null);

            // test
            var result = service.GetKaleidoscopeImagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImageConfigurationAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetKaleidoscopeImagesAsync_ImagesNull_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetImagesAsync()).ReturnsAsync((IEnumerable<Image>)null);

            // test
            var result = service.GetKaleidoscopeImagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImageConfigurationAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetKaleidoscopeImagesAsync_Success_ReturnsModel()
        {
            // setup
            repositoryMock.Setup(r => r.GetImageConfigurationAsync()).ReturnsAsync(new ImageConfiguration() { Randomize = true });
            repositoryMock.Setup(r => r.GetImagesAsync()).ReturnsAsync(new List<Image>()
            {
                new Image() { ImagePath = "Path1" },
                new Image() { ImagePath = "Path2" },
                new Image() { ImagePath = "Path3" },
                new Image() { ImagePath = "Path4" },
                new Image() { ImagePath = "Path5" },
            });

            // test
            var result = service.GetKaleidoscopeImagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetImageConfigurationAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetImagesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Image1));
            Assert.IsTrue(!string.IsNullOrEmpty(result.Image2));
            Assert.IsTrue(!string.IsNullOrEmpty(result.Image3));
            Assert.IsTrue(!string.IsNullOrEmpty(result.Image4));
            Assert.IsTrue(!string.IsNullOrEmpty(result.Image5));
        }

        [TestMethod]
        public void AssociatePlanToPurchaseAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()))
                          .ReturnsAsync(false);

            // test
            var result = service.UploadCustomerPlanAsync(id, idString).Result;

            // verify
            repositoryMock.Verify(r => r.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AssociatePlanToPurchaseAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UploadCustomerPlanAsync(id, idString).Result;

            // verify
            repositoryMock.Verify(r => r.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UploadHallOfFameAsync_GetPurchasedItemAsyncFails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.GetPurchasedItemAsync(It.IsAny<long>()))
                          .ReturnsAsync((OrderItem)null);
            // test
            var result = service.UploadHallOfFameAsync(id, idString, idString, idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchasedItemAsync(It.IsAny<long>()), Times.Once);
            repositoryMock.Verify(r => r.UpdatePurchasedItemAsync(It.IsAny<OrderItem>()), Times.Never);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UploadHallOfFameAsync_UpdatePurchasedItemAsyncFails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.UpdatePurchasedItemAsync(It.IsAny<OrderItem>()))
                          .ReturnsAsync(false);
            // test
            var result = service.UploadHallOfFameAsync(id, idString, idString, idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchasedItemAsync(It.IsAny<long>()), Times.Once);
            repositoryMock.Verify(r => r.UpdatePurchasedItemAsync(It.IsAny<OrderItem>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UploadHallOfFameAsync_Success_ReturnsTrue()
        {
            // setup
            string beforeImage = "beforeImage";
            string afterImage = "afterImage";
            string comment = "comment";

            // test
            var result = service.UploadHallOfFameAsync(id, beforeImage, afterImage, comment).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchasedItemAsync(It.IsAny<long>()), Times.Once);
            repositoryMock.Verify(r => r.UpdatePurchasedItemAsync(It.IsAny<OrderItem>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            Assert.IsNotNull(customerPlanCallback);
            Assert.AreEqual(beforeImage, customerPlanCallback.BeforeImage);
            Assert.AreEqual(afterImage, customerPlanCallback.AfterImage);
            Assert.AreEqual(comment, customerPlanCallback.Comment);
            Assert.IsTrue(customerPlanCallback.MemberOfHallOfFame);
            Assert.IsFalse(customerPlanCallback.HallOfFameEnabled);
            Assert.IsNotNull(customerPlanCallback.HallOfFameDate);
        }

        [TestMethod]
        public void GetHallOfFameEntriesAsync_NoEntries_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetHallOfFameEntriesAsync(It.IsAny<bool>(), It.IsAny<int?>()))
                          .ReturnsAsync((IEnumerable<CustomerPlan>)null);

            // test
            var result = service.GetHallOfFameEntriesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetHallOfFameEntriesAsync(It.IsAny<bool>(), It.IsAny<int?>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetHallOfFameEntriesAsync_Success()
        {
            // test
            var result = service.GetHallOfFameEntriesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetHallOfFameEntriesAsync(It.IsAny<bool>(), It.IsAny<int?>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void UpdateHallOfFameStatusAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.UpdateHallOfFameStatusAsync(It.IsAny<long>(), It.IsAny<bool>()))
                          .ReturnsAsync(false);

            // test
            var result = service.UpdateHallOfFameStatusAsync(id, true).Result;

            // verify
            repositoryMock.Verify(r => r.UpdateHallOfFameStatusAsync(It.IsAny<long>(), It.IsAny<bool>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateHallOfFameStatusAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UpdateHallOfFameStatusAsync(id, true).Result;

            // verify
            repositoryMock.Verify(r => r.UpdateHallOfFameStatusAsync(It.IsAny<long>(), It.IsAny<bool>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteHallOfFameEntryAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.DeleteHallOfFameEntryAsync(It.IsAny<long>()))
                          .ReturnsAsync(false);

            // test
            var result = service.DeleteHallOfFameEntryAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.DeleteHallOfFameEntryAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteHallOfFameEntryAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.DeleteHallOfFameEntryAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.DeleteHallOfFameEntryAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SendEmailAsync_WithoutAttachments_Fails_ReturnsFalse()
        {
            // setup
            emailServiceMock.Setup(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                            .ReturnsAsync(false);

            // test
            var result = service.SendEmailAsync("subject", "content", new List<string>() { "email1", "email2" }).Result;

            // verify
            emailServiceMock.Verify(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SendEmailAsync_WithoutAttachments_Success_ReturnsTrue()
        {
            // test
            var result = service.SendEmailAsync("subject", "content", new List<string>() { "email1", "email2" }).Result;

            // verify
            emailServiceMock.Verify(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SendEmailAsync_WithAttachments_Fails_ReturnsFalse()
        {
            // setup
            emailServiceMock.Setup(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
                            .ReturnsAsync(false);

            // test
            var result = service.SendEmailAsync("subject", "content", new List<string>() { "email1", "email2" }, new List<string>() { "attachment1", "attachment2" }).Result;

            // verify
            emailServiceMock.Verify(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SendEmailAsync_WithAttachments_Success_ReturnsTrue()
        {
            // test
            var result = service.SendEmailAsync("subject", "content", new List<string>() { "email1", "email2" }, new List<string>() { "attachment1", "attachment2" }).Result;

            // verify
            emailServiceMock.Verify(r => r.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetCustomerPlansAsync_PlanOptionsNull_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlanOptionsAsync())
                          .ReturnsAsync((IEnumerable<PlanOption>)null);
            repositoryMock.Setup(r => r.GetPurchasesAsync(It.IsAny<Guid>()))
                          .ReturnsAsync(new List<Order>() { new Order() });

            // test
            var result = service.GetCustomerPlansAsync(customerId).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlanOptionsAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetPurchasesAsync(It.IsAny<Guid>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCustomerPlansAsync_PurchasesNull_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlanOptionsAsync())
                          .ReturnsAsync(new List<PlanOption>() { new PlanOption() });
            repositoryMock.Setup(r => r.GetPurchasesAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((IEnumerable<Order>)null);

            // test
            var result = service.GetCustomerPlansAsync(customerId).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlanOptionsAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetPurchasesAsync(It.IsAny<Guid>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCustomerPlansAsync_Success_ExcludesMissingPlans_ExcludesNonCompletePurchases_ReturnsPlans()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlanOptionsAsync())
                          .ReturnsAsync(new List<PlanOption>()
                          {
                              new PlanOption(){ Id = 1, Plan = new Plan(){ Name = "Plan1", ImagePathLarge = "ImagePathLarge1" } },
                              new PlanOption(){ Id = 2, Plan = new Plan(){ Name = "Plan2", ImagePathLarge = "ImagePathLarge2" } },
                              new PlanOption(){ Id = 4, Plan = new Plan(){ Name = "Plan4", ImagePathLarge = "ImagePathLarge4" } },
                          });

            repositoryMock.Setup(r => r.GetPurchasesAsync(It.IsAny<Guid>()))
                          .ReturnsAsync(new List<Order>()
                          {
                              new Order()
                              {
                                  Id = 1,
                                  TransactionId = "tran1",
                                  QuestionnareId = 1,
                                  Status = PurchaseStatus.Complete,
                                  Items = new List<OrderItem>()
                                  {
                                      new OrderItem(){ Id = 1, /*Description = "Description1", Name = "Name1",*/ ItemCategory = ItemCategory.Plan, Price = 10 },
                                      new OrderItem(){ Id = 2, /*Description = "Description2", Name = "Name2",*/ ItemCategory = ItemCategory.Plan, Price = 20 },
                                      new OrderItem(){ Id = 3, /*Description = "Description3", Name = "Name3",*/ ItemCategory = ItemCategory.Plan, Price = 30 },
                                  },
                              },
                              new Order()
                              {
                                  Id = 3,
                                  TransactionId = "tran3",
                                  Status = PurchaseStatus.Complete,
                                  Items = new List<OrderItem>()
                                  {
                                      new OrderItem(){ Id = 4, /*Description = "Description4", Name = "Name",*/ ItemCategory = ItemCategory.Plan, Price = 10 },
                                  },
                              },
                              new Order()
                              {
                                  Id = 2,
                                  TransactionId = "tran2",
                                  Status = PurchaseStatus.Pending,
                              },
                          });

            // test
            var result = service.GetCustomerPlansAsync(customerId).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlanOptionsAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetPurchasesAsync(It.IsAny<Guid>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(0, result.Count(p => p.ItemId == 30));  // checks that the non matching plan is excluded
                                                                    // 2 purchases have questionnaire complete one doesnt
                                                                    //Assert.AreEqual(2, result.Count(p => p.QuestionnaireComplete));
                                                                    //Assert.AreEqual(1, result.Count(p => !p.QuestionnaireComplete));

            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetPurchaseSummaryAsync_NotFound_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetPurchasesAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((IEnumerable<Order>)null);

            // test
            var result = service.GetPurchaseSummaryAsync(customerId).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchasesAsync(It.IsAny<Guid>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPurchaseSummaryAsync_Success_ReturnsPurchases()
        {
            // test
            var result = service.GetPurchaseSummaryAsync(customerId).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchasesAsync(It.IsAny<Guid>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPurchaseAsync_NotFound_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetPurchasesAsync())
                          .ReturnsAsync((IEnumerable<Order>)null);

            // test
            var result = service.GetPurchasesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchasesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetPurchaseAsync_PurchaseNotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlansAsync())
                          .ReturnsAsync(new List<Plan>() { new Plan() });
            repositoryMock.Setup(r => r.GetOrdersAsync(It.IsAny<long>()))
                          .ReturnsAsync((Order)null);

            // test
            var result = service.GetPurchaseAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlansAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetOrdersAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPurchaseAsync_PlansNull_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlansAsync())
                          .ReturnsAsync((IEnumerable<Plan>)null);
            repositoryMock.Setup(r => r.GetOrdersAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Order());

            // test
            var result = service.GetPurchaseAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlansAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetOrdersAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPurchaseAsync_Success_ReturnsPurchase()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlansAsync())
                          .ReturnsAsync(new List<Plan>()
                          {
                                          new Plan(){ Name = "Plan1", Options = new List<PlanOption>(){ new PlanOption() { Id = 1 } } },
                                          new Plan(){ Options = new List<PlanOption>(){ new PlanOption() { Id = 2 } } },
                                          new Plan(){ Name = "Plan3", Options = new List<PlanOption>(){ new PlanOption() { Id = 3 } } },
                                          new Plan(){ Name = "Plan4" },
                          });
            repositoryMock.Setup(r => r.GetOrdersAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Order()
                          {
                              Customer = new Customer(),
                              DiscountCode = new DiscountCode(),
                              Questionnaire = new Questionnaire(),
                              Items = new List<OrderItem>()
                              {
                                  new OrderItem(){ Id = 1, ItemId = 1/*, Description = "Description1", Name = "Name1"*/, ItemCategory = ItemCategory.Plan, Price = 10 },
                                  new OrderItem(){ Id = 2, ItemId = 2/*, Description = "Description2", Name = "Name2"*/, ItemCategory = ItemCategory.Plan, Price = 20 },
                                  new OrderItem(){ Id = 3, ItemId = 3/*, Description = "Description3", Name = "Name3"*/, ItemCategory = ItemCategory.Plan, Price = 30 },
                              },
                          });

            // test
            var result = service.GetPurchaseAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlansAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetOrdersAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Items.Count());
            Assert.IsNotNull(result.Customer);
            Assert.IsNotNull(result.DiscountCode);
            Assert.IsNotNull(result.Questionnaire);
        }


        [TestMethod]
        public void GetPurchaseAsync_Success_OptionalPropertiesNull_ReturnsPurchase()
        {
            // setup
            repositoryMock.Setup(r => r.GetPlansAsync())
                          .ReturnsAsync(new List<Plan>() { new Plan() });
            repositoryMock.Setup(r => r.GetOrdersAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Order()
                          {
                              Customer = new Customer(),
                          });

            // test
            var result = service.GetPurchaseAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetPlansAsync(), Times.Once);
            repositoryMock.Verify(r => r.GetOrdersAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Customer);
            Assert.IsNull(result.Items);
            Assert.IsNull(result.DiscountCode);
            Assert.IsNull(result.Questionnaire);
        }

        [TestMethod]
        public void CreateMessageAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdateMessageAsync(It.IsAny<Message>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });

            // test
            var result = service.CreateMessageAsync(new CreateMessageViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateMessageAsync(It.IsAny<Message>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateMessageAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.CreateMessageAsync(new CreateMessageViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateMessageAsync(It.IsAny<Message>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void UpdateMessageAsync_Fails_ReturnsFalse()
        {
            // setup
            repositoryMock.Setup(r => r.CreateOrUpdateMessageAsync(It.IsAny<Message>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });

            // test
            var result = service.UpdateMessageAsync(new MessageViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateMessageAsync(It.IsAny<Message>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void UpdateMessageAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.UpdateMessageAsync(new MessageViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.CreateOrUpdateMessageAsync(It.IsAny<Message>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetMessagesAsync_NotFound_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetMessagesAsync())
                          .ReturnsAsync((IEnumerable<Message>)null);

            // test
            var result = service.GetMessagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetMessagesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetMessagesAsync_Success_ReturnsMessages()
        {
            // test
            var result = service.GetMessagesAsync().Result;

            // verify
            repositoryMock.Verify(r => r.GetMessagesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetMessageAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetMessageAsync(It.IsAny<long>()))
                          .ReturnsAsync((Message)null);

            // test
            var result = service.GetMessageAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetMessageAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetMessageAsync_Success_ReturnsMessage()
        {
            // test
            var result = service.GetMessageAsync(id).Result;

            // verify
            repositoryMock.Verify(r => r.GetMessageAsync(It.IsAny<long>()), Times.Once);

            Assert.IsNotNull(result);
        }

    }
}
