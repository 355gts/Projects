using System;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Common.Mapper
{
    public class Mapper : IMapper
    {
        private Dictionary<Type, Dictionary<Type, object>> Mappers { get; set; }

        public Mapper()
        {
            Mappers = new Dictionary<Type, Dictionary<Type, object>>();
        }

        public TOut Map<TIn, TOut>(TIn fromObject, TOut toObject = null) where TIn : class where TOut : class
        {
            var mapper = GetMapper(fromObject, toObject);

            if (mapper == null)
                return default(TOut);

            return mapper.Map(fromObject, toObject);
        }

        public IEnumerable<TOut> MapEnumerable<TIn, TOut>(IEnumerable<TIn> fromObjects) where TIn : class where TOut : class
        {
            var mapper = GetMapper<TIn, TOut>(fromObjects.First());

            if (mapper == null)
                return Enumerable.Empty<TOut>();

            var toObjects = new List<TOut>();

            foreach (var fromObject in fromObjects)
            {
                toObjects.Add(mapper.Map(fromObject));
            }

            return toObjects;
        }

        private ITypeMapper<TIn, TOut> GetMapper<TIn, TOut>(TIn fromObject, TOut toObject = null) where TIn : class where TOut : class
        {
            return (ITypeMapper<TIn, TOut>)Mappers[typeof(TIn)][typeof(TIn)];
        }
    }
}
