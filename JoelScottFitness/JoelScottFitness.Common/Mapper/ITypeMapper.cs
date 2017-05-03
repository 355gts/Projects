namespace JoelScottFitness.Common.Mapper
{
    public interface ITypeMapper<in TIn, TOut> where TIn : class where TOut : class
    {
        TOut Map(TIn fromObject, TOut toObject = null);
    }
}
