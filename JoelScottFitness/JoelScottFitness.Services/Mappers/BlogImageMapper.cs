using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class BlogImageMapper : ITypeMapper<BlogImageViewModel, BlogImage>, ITypeMapper<BlogImage, BlogImageViewModel>
    {
        CreateBlogImageMapper createBlogImageMapper = new CreateBlogImageMapper();

        public BlogImage Map(BlogImageViewModel fromObject, BlogImage toObject = null)
        {
            var blogImage = toObject ?? new BlogImage();

            createBlogImageMapper.Map(fromObject, blogImage);

            blogImage.BlogId = fromObject.BlogId;
            blogImage.Id = fromObject.Id;

            return blogImage;
        }

        public BlogImageViewModel Map(BlogImage fromObject, BlogImageViewModel toObject = null)
        {
            var blogImage = toObject ?? new BlogImageViewModel();

            blogImage.Caption = fromObject.Caption;
            blogImage.CaptionColour = fromObject.CaptionColour;
            blogImage.CaptionTitle = fromObject.CaptionTitle;
            blogImage.ImagePath = fromObject.ImagePath;
            blogImage.BlogId = fromObject.BlogId;
            blogImage.Id = fromObject.Id;

            return blogImage;
        }
    }
}
