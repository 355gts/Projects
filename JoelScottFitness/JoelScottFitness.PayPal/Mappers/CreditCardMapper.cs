using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using PayPal.Api;

namespace JoelScottFitness.PayPal.Mappers
{
    sealed class CreditCardMapper : ITypeMapper<CreditCardViewModel, CreditCard>
    {
        public CreditCard Map(CreditCardViewModel fromObject, CreditCard toObject = null)
        {
            var creditCard = toObject ?? new CreditCard();

            creditCard.cvv2 = fromObject.Cvv2;
            creditCard.expire_month = fromObject.ExpiryMonth;
            creditCard.expire_year = fromObject.ExpiryYear;
            creditCard.first_name = fromObject.Firstname;
            creditCard.last_name = fromObject.Surname;
            creditCard.number = fromObject.Number;
            creditCard.type = fromObject.Type.ToString().ToLower();

            return creditCard;
        }
    }
}
