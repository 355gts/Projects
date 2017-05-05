namespace JoelScottFitness.PayPal.Models
{
    public class PaymentInitiationResult
    {
        public bool Success { get; set; }

        public string PaymentId { get; set; }

        public string PayPalRedirectUrl { get; set; }
    }
}
