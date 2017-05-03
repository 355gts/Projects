using System.Collections.Generic;

namespace JoelScottFitness.Common.Mapper
{
    public interface IMapper
    {
        TOut Map<TIn, TOut>(TIn fromObject, TOut toObject = null) where TIn : class where TOut : class;

        IEnumerable<TOut> MapEnumerable<TIn, TOut>(IEnumerable<TIn> fromObjects) where TIn : class where TOut : class;
    }
}
