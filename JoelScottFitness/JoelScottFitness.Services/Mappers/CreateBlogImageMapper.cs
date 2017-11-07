using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateBlogImageMapper : ITypeMapper<CreateBlogImageViewModel, BlogImage>
    {
        public BlogImage Map(CreateBlogImageViewModel fromObject, BlogImage toObject = null)
        {
            var blogImage = toObject ?? new BlogImage();

            blogImage.Caption = fromObject.Caption;
            blogImage.CaptionColour = fromObject.CaptionColour;
            blogImage.CaptionTitle = fromObject.CaptionTitle;
            blogImage.ImagePath = fromObject.ImagePath;

            return blogImage;
        }
    }
}
