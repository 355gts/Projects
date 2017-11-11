using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using Repo = JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class ImageMapper : ITypeMapper<Repo.Image, ImageViewModel>
    {
        public ImageViewModel Map(Repo.Image fromObject, ImageViewModel toObject = null)
        {
            var image = toObject ?? new ImageViewModel();

            image.Id = fromObject.Id;
            image.ImagePath = fromObject.ImagePath;

            return image;
        }
    }
}
