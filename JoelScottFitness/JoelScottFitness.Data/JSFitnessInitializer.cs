using JoelScottFitness.Data.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace JoelScottFitness.Data
{
    public class JSFitnessInitializer : CreateDatabaseIfNotExists<JSFitnessContext>
    {
        protected override void Seed(JSFitnessContext context)
        {
            if (!context.Blogs.Any())
            {
                var blog1 = new Blog()
                {
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Desert.jpg",
                    SubHeader = "Welcome to Joel Scott Fitness",
                    Title = "My First Blog",
                };
                var blog2 = new Blog()
                {
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Chrys.jpg",
                    SubHeader = "My Second Blog",
                    Title = "Here We Go Again!",
                };
                var blog3 = new Blog()
                {
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Desert.jpg",
                    SubHeader = "Welcome to Joel Scott Fitness",
                    Title = "My First Blog",
                };
                var blog4 = new Blog()
                {
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Chrys.jpg",
                    SubHeader = "My Second Blog",
                    Title = "Here We Go Again!",
                };
                var blog5 = new Blog()
                {
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Desert.jpg",
                    SubHeader = "Welcome to Joel Scott Fitness",
                    Title = "My First Blog",
                };
                var blog6 = new Blog()
                {
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Chrys.jpg",
                    SubHeader = "My Second Blog",
                    Title = "Here We Go Again!",
                };

                context.Blogs.Add(blog1);
                context.Blogs.Add(blog2);
                context.Blogs.Add(blog3);
                context.Blogs.Add(blog4);
                context.Blogs.Add(blog5);
                context.Blogs.Add(blog6);
                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}
