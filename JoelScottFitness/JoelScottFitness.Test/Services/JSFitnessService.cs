using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data;
using JoelScottFitness.Data.Enumerations;
using JoelScottFitness.Data.Models;
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
            repositoryMock.Setup(r => r.SavePurchaseAsync(It.IsAny<Purchase>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true });
            repositoryMock.Setup(r => r.UpdatePurchaseStatusAsync(It.IsAny<string>(), It.IsAny<PurchaseStatus>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetPurchaseByTransactionIdAsync(It.IsAny<string>()))
                          .ReturnsAsync(new Purchase());
            repositoryMock.Setup(r => r.CreateOrUpdateQuestionnaireAsync(It.IsAny<Questionnaire>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = true });
            repositoryMock.Setup(r => r.AssociateQuestionnaireToPurchaseAsync(It.IsAny<long>(), It.IsAny<long>()))
                          .ReturnsAsync(true);
            repositoryMock.Setup(r => r.GetQuestionnaireAsync(It.IsAny<long>()))
                          .ReturnsAsync(new Questionnaire());
            repositoryMock.Setup(r => r.UpdatePlanStatusAsync(It.IsAny<long>(), It.IsAny<bool>()))
                          .ReturnsAsync(true);

            paypalServiceMock.Setup(r => r.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(new PaymentResult() { Success = true });
            paypalServiceMock.Setup(r => r.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()))
                             .Returns(new PaymentInitiationResult() { Success = true });

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

        [TestMethod]
        public void GetBasketItemsAsync_NotFound_ReturnsEmptyList()
        {
            // setup
            repositoryMock.Setup(r => r.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()))
                          .ReturnsAsync((IEnumerable<PlanOption>)null);
            // test
            var result = service.GetBasketItemsAsync(new List<long>() { id }).Result;

            // verify
            repositoryMock.Verify(r => r.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetBasketItemsAsync_Success_ReturnsBasketItems()
        {
            // test
            var result = service.GetBasketItemsAsync(new List<long>() { id }).Result;

            // verify
            repositoryMock.Verify(r => r.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetUserAsync_NotFound_ReturnsNull()
        {
            throw new NotImplementedException();
            // setup
            //repositoryMock.Setup(r => r.GetUserAsync(It.IsAny<string>()))
            //              .ReturnsAsync((AuthUser)null);

            // test
            var result = service.GetUserAsync(idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetUserAsync(It.IsAny<string>()), Times.Once);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserAsync_Found_ReturnsUser()
        {
            throw new NotImplementedException();
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
            repositoryMock.Setup(r => r.SavePurchaseAsync(It.IsAny<Purchase>()))
                          .ReturnsAsync(new AsyncResult<long>() { Success = false });
            // test
            var result = service.SavePurchaseAsync(new ConfirmPurchaseViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.SavePurchaseAsync(It.IsAny<Purchase>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void SavePurchaseAsync_Success_ReturnsTrue()
        {
            // test
            var result = service.SavePurchaseAsync(new ConfirmPurchaseViewModel()).Result;

            // verify
            repositoryMock.Verify(r => r.SavePurchaseAsync(It.IsAny<Purchase>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
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
        public void GetPurchaseByTransactionIdAsync_NotFound_ReturnsNull()
        {
            // setup
            repositoryMock.Setup(r => r.GetPurchaseByTransactionIdAsync(It.IsAny<string>()))
                          .ReturnsAsync((Purchase)null);

            // test
            var result = service.GetPurchaseByTransactionIdAsync(idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchaseByTransactionIdAsync(It.IsAny<string>()), Times.Once);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPurchaseByTransactionIdAsync_Success_ReturnsPurchase()
        {
            // test
            var result = service.GetPurchaseByTransactionIdAsync(idString).Result;

            // verify
            repositoryMock.Verify(r => r.GetPurchaseByTransactionIdAsync(It.IsAny<string>()), Times.Once);

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
    }
}
