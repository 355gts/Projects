using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateBlogMapper : ITypeMapper<CreateBlogViewModel, Blog>
    {
        CreateBlogImageMapper blogImageMapper = new CreateBlogImageMapper();

        public Blog Map(CreateBlogViewModel fromObject, Blog toObject = null)
        {
            var blog = toObject ?? new Blog();

            blog.Content = fromObject.Content;
            blog.ImagePath = fromObject.ImagePath;
            blog.SubHeader = fromObject.SubHeader;
            blog.Title = fromObject.Title;
            blog.Active = false;
            blog.CreatedDate = DateTime.UtcNow;
            
            var blogImages = new List<BlogImage>();
            if (blog.BlogImages != null)
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
