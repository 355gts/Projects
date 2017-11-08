using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class BlogMapper : ITypeMapper<Blog, BlogViewModel>, ITypeMapper<BlogViewModel, Blog>
    {
        BlogImageMapper blogImageMapper = new BlogImageMapper();

        public BlogViewModel Map(Blog fromObject, BlogViewModel toObject = null)
        {
            var blog = toObject ?? new BlogViewModel();
            
            blog.Content = fromObject.Content;
            blog.CreatedDate = fromObject.CreatedDate;
            blog.ModifiedDate = fromObject.ModifiedDate;
            blog.Id = fromObject.Id;
            blog.ImagePath = fromObject.ImagePath;
            blog.SubHeader = fromObject.SubHeader;
            blog.Title = fromObject.Title;
            blog.Active = fromObject.Active;

            var blogImages = new List<BlogImageViewModel>();
            if (fromObject.BlogImages != null)
            {
                foreach (var blogImage in fromObject.BlogImages)
                {
                    blogImages.Add(blogImageMapper.Map(blogImage));
                }                
            }
            blog.BlogImages = blogImages;

            return blog;
        }

        public Blog Map(BlogViewModel fromObject, Blog toObject = null)
        {
            var blog = toObject ?? new Blog();
            
            blog.Content = fromObject.Content;
            blog.CreatedDate = fromObject.CreatedDate;
            blog.ModifiedDate = DateTime.UtcNow;
            blog.Id = fromObject.Id;
            blog.ImagePath = fromObject.ImagePath;
            blog.SubHeader = fromObject.SubHeader;
            blog.Title = fromObject.Title;
            blog.Active = fromObject.Active;

            var blogImages = new List<BlogImage>();
            if (fromObject.BlogImages != null)
            {
                foreach (var blogImage in fromObject.BlogImages)
                {
                    blogImages.Add(blogImageMapper.Map(blogImage));
                }
            }
            blog.BlogImages = blogImages;

            return blog;
        }
    }
}
