using JoelScottFitness.Common.Enumerations;

namespace JoelScottFitness.Common.Models
{
    public class CreditCardViewModel
    {
        public string Firstname { get; set; }

        public string Surname { get; set; }

        public string Number { get; set; }

        public string Cvv2 { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public CreditCardType Type { get; set; }
    }
}
