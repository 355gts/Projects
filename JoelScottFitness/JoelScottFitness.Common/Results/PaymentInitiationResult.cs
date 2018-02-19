namespace JoelScottFitness.Common.Results
{
    public class PaymentInitiationResult
    {
        public bool Success { get; set; }

        public string PaymentId { get; set; }

        public string PayPalRedirectUrl { get; set; }

        public string TransactionId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
