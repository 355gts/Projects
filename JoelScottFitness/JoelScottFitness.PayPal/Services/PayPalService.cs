using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.PayPal.Configuration;
using log4net;
using Ninject;
using PayPal;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JoelScottFitness.PayPal.Services
{
    public class PayPalService : IPayPalService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PayPalService));

        private readonly IMapper mapper;
        
        private ItemList itemList;
        private Address billingAddress;
        private CreditCard creditCard;
        private Details details;
        private Amount amount;
        private Transaction transaction;
        private List<Transaction> transactions;
        private FundingInstrument fundingInstrument;
        private List<FundingInstrument> fundingInstruments;
        private Payer payer;

        public PayPalService() 
            : this(new Mapper(Assembly.Load("JoelScottFitness.PayPal")))
        {

        }
        public PayPalService([Named("PayPalMapper")] IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            this.mapper = mapper;

            InitialisePayment();
        }

        public void InitialisePayment()
        {
            itemList = new ItemList();
            itemList.items = new List<Item>();
            billingAddress = new Address();
            creditCard = new CreditCard();
            details = new Details();
            amount = new Amount();
            transaction = new Transaction();
            transactions = new List<Transaction>();
            fundingInstrument = new FundingInstrument();
            fundingInstruments = new List<FundingInstrument>();
            payer = new Payer();
        }

        public void AddCreditCard(CreditCardViewModel newCreditCard)
        {
            creditCard = mapper.Map<CreditCardViewModel, CreditCard>(newCreditCard);
            creditCard.billing_address = billingAddress;

            fundingInstrument.credit_card = creditCard;

            fundingInstruments.Add(fundingInstrument);

            payer.funding_instruments = fundingInstruments;
            payer.payment_method = "credit_card";
        }

        public void AddItem(SelectedPlanOptionViewModel item)
        {
            var paypalItem = mapper.Map<SelectedPlanOptionViewModel, Item>(item);

            if (!itemList.items.Contains(paypalItem))
                itemList.items.Add(paypalItem);
        }

        public void AddItems(IEnumerable<SelectedPlanOptionViewModel> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public PaymentResult PayWithCreditCard()
        {
            Payment payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactions
            };

            try
            {
                APIContext apiContext = PayPalConfiguration.GetAPIContext();

                Payment createdPayment = payment.Create(apiContext);

                if (createdPayment.state.ToLower() != "approved")
                    return new PaymentResult()
                    {
                        Success = false,
                        ErrorMessage = $"Payment was not approvred, returned state '{createdPayment.state}'.",
                    };
            }
            catch(PayPalException pex)
            {
                logger.Warn($"PayPal error - '{pex.Message}'.");
                return new PaymentResult()
                {
                    Success = false,
                    ErrorMessage = $"PayPal error - '{pex.Message}'."
                };
            }

            return new PaymentResult() { Success = true };
        }

        public void RemoveAllItems()
        {
            itemList.items = new List<Item>();
        }

        public void RemoveItem(PlanOptionViewModel item)
        {
            var paypalItem = mapper.Map<PlanOptionViewModel, Item>(item);

            if (itemList.items.Contains(paypalItem))
            {
                itemList.items.Remove(paypalItem);
            }
        }

        public void SetBillingAddress(AddressViewModel address)
        {
            billingAddress = mapper.Map<AddressViewModel, Address>(address);
        }

        public void SetPaymentDetails(double total)
        {
            details.subtotal = total.ToString();

            amount.currency = "GBP";
            amount.details = details;
            amount.total = total.ToString();
        }

        public void CreateTransaction(string description, string invoiceNumber)
        {
            transaction.amount = amount;
            transaction.description = description;
            transaction.item_list = itemList;
            transaction.invoice_number = invoiceNumber;

            transactions.Add(transaction);
        }

        public void AddPayPalPayer()
        {
            payer = new Payer() { payment_method = "paypal" };
        }


        public PaymentInitiationResult InitiatePayPalPayment(ConfirmPurchaseViewModel confirmPurchaseViewModel, string baseUri)
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();

            //string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
            //            "/Home/CompletePayment?";

            var transactionId = Convert.ToString((new Random()).Next(100000));

            // add items to transaction
            AddItems(confirmPurchaseViewModel.BasketItems);
            SetBillingAddress(confirmPurchaseViewModel.CustomerDetails.BillingAddress);
            
            var createdPayment = this.CreatePayment(apiContext, transactionId, baseUri + "guid=" + transactionId);

            var approvalLink = createdPayment.links
                                             .Where(l => l.rel.ToLower().Trim() == "approval_url")
                                             .Select(l => l.href).FirstOrDefault();

            if (createdPayment == null || string.IsNullOrEmpty(createdPayment.id) || string.IsNullOrEmpty(approvalLink))
            {
                return new PaymentInitiationResult()
                {
                    Success = false,
                };
            }
            
            return new PaymentInitiationResult()
            {
                Success = true,
                PayPalRedirectUrl = approvalLink,
                PaymentId = createdPayment.id,
                TransactionId = transactionId,
            };
        }
        
        public PaymentResult CompletePayPalPayment(string paymentId, string payerId)
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();

            var executedPayment = ExecutePayment(apiContext, payerId, paymentId);

            if (executedPayment.state.ToLower() != "approved")
            {
                return new PaymentResult()
                {
                    Success = false,
                    ErrorMessage = $"Payment was not approvied, returned state of '{executedPayment.state}'.",
                };
            }

            return new PaymentResult()
            {
                Success = true,
            };
        }
        
        private Payment CreatePayment(APIContext apiContext, string transactionId, string redirectUrl)
        {
            var totalCost = itemList.items.Select(i => Convert.ToDouble(i.price) * Convert.ToInt32(i.quantity)).Sum().ToString();
            
            var payer = new Payer() { payment_method = "paypal" };
            
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var details = new Details()
            {
                subtotal = totalCost,
            };
            
            var amount = new Amount()
            {
                currency = "GBP",
                total = totalCost,
                details = details
            };

            var transactionList = new List<Transaction>
            {
                new Transaction()
                {
                    description = "Joel Scott Fitness",
                    invoice_number = transactionId,
                    amount = amount,
                    item_list = itemList,
                }
            };

            Payment payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls,
            };

            return payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            Payment payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }
    }
}
