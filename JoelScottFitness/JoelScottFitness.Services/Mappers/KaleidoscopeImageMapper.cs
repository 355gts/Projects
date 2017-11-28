using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class KaleidoscopeImageMapper : ITypeMapper<ImageConfiguration, KaleidoscopeViewModel>
    {
        public KaleidoscopeViewModel Map(ImageConfiguration fromObject, KaleidoscopeViewModel toObject = null)
        {
            var kaleidoscopeImageConfiguration = toObject ?? new KaleidoscopeViewModel();

            var imageIds = GetRandomImageIds(fromObject.Images, 5);
            var imageArray = fromObject.Images.ToArray();

            kaleidoscopeImageConfiguration.Image1 = imageArray[imageIds[0]].ImagePath;
            kaleidoscopeImageConfiguration.Image2 = imageArray[imageIds[1]].ImagePath;
            kaleidoscopeImageConfiguration.Image3 = imageArray[imageIds[2]].ImagePath;
            kaleidoscopeImageConfiguration.Image4 = imageArray[imageIds[3]].ImagePath;
            kaleidoscopeImageConfiguration.Image5 = imageArray[imageIds[4]].ImagePath;

            return kaleidoscopeImageConfiguration;
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
