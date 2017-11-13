using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class SectionImageConfigurationMapper : ITypeMapper<ImageConfiguration, SectionImageViewModel>
    {
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
