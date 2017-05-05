using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class BlogMapper : ITypeMapper<Blog, BlogViewModel>, ITypeMapper<BlogViewModel, Blog>
    {
        public BlogViewModel Map(Blog fromObject, BlogViewModel toObject = null)
        {
            var blog = toObject ?? new BlogViewModel();

            blog.Active = fromObject.Active;
            blog.ActiveFrom = fromObject.ActiveFrom;
            blog.ActiveTo = fromObject.ActiveTo;
            blog.Content = fromObject.Content;
            blog.CreatedDate = fromObject.CreatedDate;
            blog.Id = fromObject.Id;
            blog.ImagePath = fromObject.ImagePath;
            blog.SubHeader = fromObject.SubHeader;
            blog.Title = fromObject.Title;

            return blog;
        }

        public Blog Map(BlogViewModel fromObject, Blog toObject = null)
        {
            var blog = toObject ?? new Blog();
            
            blog.ActiveFrom = fromObject.ActiveFrom;
            blog.ActiveTo = fromObject.ActiveTo;
            blog.Content = fromObject.Content;
            blog.CreatedDate = fromObject.CreatedDate;
            blog.Id = fromObject.Id;
            blog.ImagePath = fromObject.ImagePath;
            blog.SubHeader = fromObject.SubHeader;
            blog.Title = fromObject.Title;

            return blog;
        }
    }
}
