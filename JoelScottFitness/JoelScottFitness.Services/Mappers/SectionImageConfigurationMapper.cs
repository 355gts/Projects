using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
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
                var imageIds = GetRandomImageIds(fromObject.Images, 4);
                var imageArray = fromObject.Images.ToArray();
                
                sectionImageConfiguration.SectionImage1 = imageArray[imageIds[0]].ImagePath;
                sectionImageConfiguration.SectionImage2 = imageArray[imageIds[1]].ImagePath;
                sectionImageConfiguration.SectionImage3 = imageArray[imageIds[2]].ImagePath;
                sectionImageConfiguration.SplashImage = imageArray[imageIds[3]].ImagePath;
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

        private int[] GetRandomImageIds(IEnumerable<Image> images, int upperBound)
        {
            Random randomSelector = new Random();
            int imageCount = images.Count();
            IList<int> imageIds = new List<int>();

            while (imageIds.Count() < upperBound)
            {
                int imageId = randomSelector.Next(0, imageCount);

                if (!imageIds.Contains(imageId))
                    imageIds.Add(imageId);
            }

            return imageIds.ToArray();
        }
    }
}
