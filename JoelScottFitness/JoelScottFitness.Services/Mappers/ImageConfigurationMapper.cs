using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class ImageConfigurationMapper : ITypeMapper<ImageConfigurationViewModel, ImageConfiguration>, ITypeMapper<ImageConfiguration, ImageConfigurationViewModel>
    {
        public ImageConfiguration Map(ImageConfigurationViewModel fromObject, ImageConfiguration toObject = null)
        {
            var imageConfiguration = toObject ?? new ImageConfiguration();

            imageConfiguration.Randomize = fromObject.Randomize;
            imageConfiguration.SectionImage1Id = fromObject.SectionImage1Id;
            imageConfiguration.SectionImage2Id = fromObject.SectionImage2Id;
            imageConfiguration.SectionImage3Id = fromObject.SectionImage3Id;
            imageConfiguration.SplashImageId = fromObject.SplashImageId;

            return imageConfiguration;
        }

        public ImageConfigurationViewModel Map(ImageConfiguration fromObject, ImageConfigurationViewModel toObject = null)
        {
            var imageConfiguration = toObject ?? new ImageConfigurationViewModel();

            imageConfiguration.Randomize = fromObject.Randomize;
            imageConfiguration.SectionImage1Id = fromObject.SectionImage1Id;
            imageConfiguration.SectionImage2Id = fromObject.SectionImage2Id;
            imageConfiguration.SectionImage3Id = fromObject.SectionImage3Id;
            imageConfiguration.SplashImageId = fromObject.SplashImageId;

            return imageConfiguration;
        }
    }
}
