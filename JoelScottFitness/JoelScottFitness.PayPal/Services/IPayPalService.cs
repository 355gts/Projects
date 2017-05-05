using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using System.Collections.Generic;

namespace JoelScottFitness.PayPal.Services
{
    public interface IPayPalService
    {
        void InitialisePayment();

        void AddItems(IEnumerable<PurchasedItemViewModel> items);

        void AddItem(PurchasedItemViewModel item);

        void RemoveItem(PurchasedItemViewModel item);

        void RemoveAllItems();

        void SetBillingAddress(AddressViewModel address);

        void AddCreditCard(CreditCardViewModel creditCard);

        void SetPaymentDetails(double total);

        void CreateTransaction(string description, string invoiceNumber);

        PaymentResult PayWithCreditCard();

        PaymentInitiationResult InitiatePayPalPayment(string baseUri);
        
        PaymentResult CompletePayPalPayment(string paymentId, string payerId);
    }
}
