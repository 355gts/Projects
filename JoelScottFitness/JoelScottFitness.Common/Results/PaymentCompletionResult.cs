namespace JoelScottFitness.Common.Results
{
    public class PaymentCompletionResult
    {
        public bool Success { get; set; }

        public string PaymentId { get; set; }

        public string PayerId { get; set; }

        public string TransactionId { get; set; }

        public long PurchaseId { get; set; }
    }
}
