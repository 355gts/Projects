using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class KaleidoscopeImageMapper : ITypeMapper<ImageConfiguration, KaleidoscopeViewModel>
    {
        public KaleidoscopeViewModel Map(ImageConfiguration fromObject, KaleidoscopeViewModel toObject = null)
        {
            var kaleidoscopeImageConfiguration = toObject ?? new KaleidoscopeViewModel();
            
            Random randomSelector = new Random();
            int imageCount = fromObject.Images.Count();
            var imageArray = fromObject.Images.ToArray();

            kaleidoscopeImageConfiguration.Image1 = imageArray[randomSelector.Next(0, imageCount)].ImagePath;
            kaleidoscopeImageConfiguration.Image2 = imageArray[randomSelector.Next(0, imageCount)].ImagePath;
            kaleidoscopeImageConfiguration.Image3 = imageArray[randomSelector.Next(0, imageCount)].ImagePath;
            kaleidoscopeImageConfiguration.Image4 = imageArray[randomSelector.Next(0, imageCount)].ImagePath;
            kaleidoscopeImageConfiguration.Image5 = imageArray[randomSelector.Next(0, imageCount)].ImagePath;

            return kaleidoscopeImageConfiguration;
        }
    }
}
