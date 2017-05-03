namespace JoelScottFitness.Common.Results
{
    public class AsyncResult<TType>
    {
        public bool Success { get; set; }

        public TType Result { get; set; }

        public AsyncResult()
        {
            
        }
    }
}
