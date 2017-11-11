using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class ImageConfigurationMapper : ITypeMapper<ImageConfigurationViewModel, ImageConfiguration>, ITypeMapper<ImageConfiguration, SectionImageViewModel>, ITypeMapper<ImageConfiguration, ImageConfigurationViewModel>
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

        public SectionImageViewModel Map(ImageConfiguration fromObject, SectionImageViewModel toObject = null)
        {
            var sectionImageConfiguration = toObject ?? new SectionImageViewModel();

            // randomly select images or user pre-configured ones
            if (fromObject.Randomize)
            {
                Random randomSelector = new Random();
                int imageCount = fromObject.Images.Count();
                var imageArray = fromObject.Images.ToArray();

                sectionImageConfiguration.SectionImage1 = imageArray[randomSelector.Next(0, imageCount)].ImagePath;
                sectionImageConfiguration.SectionImage2 = imageArray[randomSelector.Next(0, imageCount)].ImagePath;
                sectionImageConfiguration.SectionImage3 = imageArray[randomSelector.Next(0, imageCount)].ImagePath;
                sectionImageConfiguration.SplashImage = imageArray[randomSelector.Next(0, imageCount)].ImagePath;
            }
            else
            {
                sectionImageConfiguration.SectionImage1 = fromObject.Images.FirstOrDefault(i => i.Id == fromObject.SectionImage1Id)?.ImagePath;
                sectionImageConfiguration.SectionImage2 = fromObject.Images.FirstOrDefault(i => i.Id == fromObject.SectionImage2Id)?.ImagePath;
                sectionImageConfiguration.SectionImage3 = fromObject.Images.FirstOrDefault(i => i.Id == fromObject.SectionImage3Id)?.ImagePath;
                sectionImageConfiguration.SplashImage = fromObject.Images.FirstOrDefault(i => i.Id == fromObject.SplashImageId)?.ImagePath;
            }

            return sectionImageConfiguration;
        }
    }
}
