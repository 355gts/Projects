using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoelScottFitness.PayPal.Models;
using PayPal.Api;
using PayPal;
using log4net;
using JoelScottFitness.PayPal.Configuration;

namespace JoelScottFitness.PayPal.Services
{
    public class PayPalService : IPayPalService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PayPalService));

        private List<Item> items;
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
        {
            InitialisePayment();
        }

        public void InitialisePayment()
        {
            items = new List<Item>();
            itemList = new ItemList();
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

        public void AddCreditCard(CreditCard newCreditCard)
        {
            creditCard = newCreditCard;
            creditCard.billing_address = billingAddress;

            fundingInstrument.credit_card = creditCard;

            fundingInstruments.Add(fundingInstrument);

            payer.funding_instruments = fundingInstruments;
            payer.payment_method = "credit_card";
        }

        public void AddItem(Item item)
        {
            if (!items.Contains(item))
                items.Add(item);
        }

        public void AddItems(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public PayPalPaymentResult PayWithCreditCard()
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
                    return new PayPalPaymentResult()
                    {
                        Success = false,
                        ErrorMessage = $"Payment was not approvred, returned state '{createdPayment.state}'.",
                    };
            }
            catch(PayPalException pex)
            {
                logger.Warn($"PayPal error - '{pex.Message}'.");
                return new PayPalPaymentResult()
                {
                    Success = false,
                    ErrorMessage = $"PayPal error - '{pex.Message}'."
                };
            }

            return new PayPalPaymentResult() { Success = true };
        }

        public void RemoveAllItems()
        {
            items = new List<Item>();
        }

        public void RemoveItem(Item item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
            }
        }

        public void SetBillingAddress(Address address)
        {
            billingAddress = address;
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
            itemList.items = items;

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


        public PaymentInitiationResult InitialPayPalPayment(string baseUri)
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();

            //string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
            //            "/Home/CompletePayment?";

            var guid = Convert.ToString((new Random()).Next(100000));

            var createdPayment = this.CreatePayment(apiContext, baseUri + "guid=" + guid);

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
            };
        }
        
        public PayPalPaymentResult CompletePayPalPayment(string PaymentId, string payerId)
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();

            var executedPayment = ExecutePayment(apiContext, payerId, PaymentId);

            if (executedPayment.state.ToLower() != "approved")
            {
                return new PayPalPaymentResult()
                {
                    Success = false,
                    ErrorMessage = $"Payment was not approvied, returned state of '{executedPayment.state}'.",
                };
            }

            return new PayPalPaymentResult()
            {
                Success = true
            };
        }
        
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var totalCost = items.Select(i => Convert.ToDouble(i.price) * Convert.ToInt32(i.quantity)).Sum().ToString();

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

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "your invoice number",
                amount = amount,
                item_list = itemList
            });

            Payment payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
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
