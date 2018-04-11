using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using System.Collections.Generic;

namespace JoelScottFitness.PayPal.Services
{
    public interface IPayPalService
    {
        void InitialisePayment();

        void AddItems(IEnumerable<BasketItemViewModel> items);

        void AddItem(BasketItemViewModel item);

        void RemoveItem(BasketItemViewModel item);

        void RemoveAllItems();

        void SetBillingAddress(AddressViewModel address);

        void AddCreditCard(CreditCardViewModel creditCard);

        void SetPaymentDetails(double total);

        void CreateTransaction(string description, string invoiceNumber);

        PaymentResult PayWithCreditCard();

        PaymentInitiationResult InitiatePayPalPayment(ConfirmOrderViewModel confirmOrderViewModel, string baseUri);

        PaymentResult CompletePayPalPayment(string paymentId, string payerId);
    }
}
