using JoelScottFitness.PayPal.Models;
using PayPal.Api;
using System.Collections.Generic;

namespace JoelScottFitness.PayPal.Services
{
    public interface IPayPalService
    {
        void InitialisePayment();

        void AddItems(IEnumerable<Item> items);

        void AddItem(Item item);

        void RemoveItem(Item item);

        void RemoveAllItems();

        void SetBillingAddress(Address address);

        void AddCreditCard(CreditCard creditCard);

        void SetPaymentDetails(double total);

        void CreateTransaction(string description, string invoiceNumber);

        PayPalPaymentResult PayWithCreditCard();

        PaymentInitiationResult InitialPayPalPayment(string baseUri);

        PayPalPaymentResult CompletePayPalPayment(string PaymentId, string payerId);
    }
}
