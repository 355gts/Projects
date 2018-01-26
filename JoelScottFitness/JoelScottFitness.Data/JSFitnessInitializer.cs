using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity.Models;
using System;
using System.Collections.Generic;
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
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Blogs/Desert.jpg",
                    SubHeader = "Welcome to Joel Scott Fitness",
                    Title = "My First Blog",
                    Active = true,
                    BlogImages = new List<BlogImage>()
                    {
                        new BlogImage()
                        {
                            Caption = "Welcome to my first blog image",
                            CaptionColour = BlogCaptionTextColour.White,
                            CaptionTitle = "My First Blog Image",
                            ImagePath = "/Content/Images/Blogs/ladies1.jpg",
                        },

                        new BlogImage()
                        {
                            Caption = "Welcome to my second blog image",
                            CaptionColour = BlogCaptionTextColour.White,
                            CaptionTitle = "My Second Blog Image",
                            ImagePath = "/Content/Images/Blogs/ladies2.jpg",
                        },

                        new BlogImage()
                        {
                            Caption = "This was interesting",
                            CaptionColour = BlogCaptionTextColour.White,
                            CaptionTitle = "My Third Blog Image",
                            ImagePath = "/Content/Images/Blogs/ladies3.jpg",
                        },
                    },
                };
                var blog2 = new Blog()
                {
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Blogs/Chrys.jpg",
                    SubHeader = "My Second Blog",
                    Title = "Here We Go Again!",
                    Active = true,
                };
                var blog3 = new Blog()
                {
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Blogs/Desert.jpg",
                    SubHeader = "Welcome to Joel Scott Fitness",
                    Title = "My First Blog",
                    Active = true,
                };
                var blog4 = new Blog()
                {
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Blogs/Chrys.jpg",
                    SubHeader = "My Second Blog",
                    Title = "Here We Go Again!",
                    Active = true,
                };
                var blog5 = new Blog()
                {
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Blogs/Desert.jpg",
                    SubHeader = "Welcome to Joel Scott Fitness",
                    Title = "My First Blog",
                    Active = true,
                };
                var blog6 = new Blog()
                {
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.",
                    CreatedDate = DateTime.UtcNow,
                    ImagePath = "/Content/Images/Blogs/Chrys.jpg",
                    SubHeader = "My Second Blog",
                    Title = "Here We Go Again!",
                    Active = true,
                };

                context.Blogs.Add(blog1);
                context.Blogs.Add(blog2);
                context.Blogs.Add(blog3);
                context.Blogs.Add(blog4);
                context.Blogs.Add(blog5);
                context.Blogs.Add(blog6);
                context.SaveChanges();
            }

            if (!context.Plans.Any())
            {
                var plan1 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Get Leaner And Stronger",
                    CreatedDate = DateTime.UtcNow,
                    Description = "It doesn't matter what shape you're in, your age, or how many diet programs you've tried. You can transform your body and become leaner and stronger",
                    ImagePathLarge = "/Content/Images/Plans/male-plan1.jpg",
                    ImagePathMedium = "/Content/Images/Plans/male-plan1.jpg",
                    Name = "Lean Body",
                    TargetGender = Gender.Male,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "12 Week Daily Trainer",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 28.99,
                        },

                        new PlanOption()
                        {
                            Description = "18 Week Daily Trainer",
                            Duration = 18,
                            ItemType = ItemType.Plan,
                            Price = 38.99,
                        },

                        new PlanOption()
                        {
                            Description = "24 Week Daily Trainer",
                            Duration = 24,
                            ItemType = ItemType.Plan,
                            Price = 45.99,
                        },
                    },
                };
                var plan2 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Get Shredded!",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Get ready to burn fat, build muscle, boost strength, and get absolutely shredded in only six weeks. Get ready to achieve the best shape of your life. Get ready for Shortcut to Shred.",
                    ImagePathLarge = "/Content/Images/Plans/male-plan2.jpg",
                    ImagePathMedium = "/Content/Images/Plans/male-plan2.jpg",
                    Name = "Shortcut to Shred",
                    TargetGender = Gender.Male,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "6 Week Shred",
                            Duration = 6,
                            ItemType = ItemType.Plan,
                            Price = 18.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Shred",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 25.99,
                        },
                        new PlanOption()
                        {
                            Description = "24 Week Shred",
                            Duration = 24,
                            ItemType = ItemType.Plan,
                            Price = 45.99,
                        },
                    },
                };
                var plan3 = new Plan()
                {
                    Active = true,
                    BannerHeader = "The Total-Body Fitness Plan",
                    CreatedDate = DateTime.UtcNow,
                    Description = "You want it all: a shredded, muscular physique; strength that can be expressed through explosive power; and a racecar engine under the hood. To get there, training the same old way won’t suffice. Ripped Remix is an intense 4-week training plan designed to help you master your body, shake up your workouts, torch fat, and build new levels of strength.",
                    ImagePathLarge = "/Content/Images/Plans/male-plan3.jpg",
                    ImagePathMedium = "/Content/Images/Plans/male-plan3.jpg",
                    Name = "Ripped",
                    TargetGender = Gender.Male,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "28 Day Plan",
                            Duration = 4,
                            ItemType = ItemType.Plan,
                            Price = 18.99,
                        },
                        new PlanOption()
                        {
                            Description = "42 Day Plan",
                            Duration = 6,
                            ItemType = ItemType.Plan,
                            Price = 25.99,
                        },
                        new PlanOption()
                        {
                            Description = "58 Day Plan",
                            Duration = 8,
                            ItemType = ItemType.Plan,
                            Price = 30.99,
                        },
                    },
                };
                var plan4 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Muscle-Building Trainer",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Project Mass is a cutting-edge training, nutrition, and supplement program designed to help you build maximum size. This is how you grow.",
                    ImagePathLarge = "/Content/Images/Plans/male-plan4.jpg",
                    ImagePathMedium = "/Content/Images/Plans/male-plan4.jpg",
                    Name = "Mass Effect",
                    TargetGender = Gender.Male,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "14 Week Plan",
                            Duration = 14,
                            ItemType = ItemType.Plan,
                            Price = 45.99,
                        },
                        new PlanOption()
                        {
                            Description = "21 Week Plan",
                            Duration = 6,
                            ItemType = ItemType.Plan,
                            Price = 60.99,
                        },
                        new PlanOption()
                        {
                            Description = "28 Week Plan",
                            Duration = 8,
                            ItemType = ItemType.Plan,
                            Price = 65.99,
                        },
                    },
                };
                var plan5 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Become a Triple Threat!",
                    CreatedDate = DateTime.UtcNow,
                    Description = "You don't have to choose between muscle, strength, and conditioning. You can train to be both athlete and Adonis, functional and ferocious. Harness the strength that comes from wielding the weights, while creating an aerobic engine to power you through any challenge.",
                    ImagePathLarge = "/Content/Images/Plans/female-plan1.jpg",
                    ImagePathMedium = "/Content/Images/Plans/female-plan1.jpg",
                    Name = "Triple Threat",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 4,
                            ItemType = ItemType.Plan,
                            Price = 20.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 6,
                            ItemType = ItemType.Plan,
                            Price = 30.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 27.99,
                        },
                    },
                };
                var plan6 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Hit the Beach like never before",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Have you been endlessly searching for workout tips and plans to achieve your fitness goals? Well look no further with the Bikini Body Guide- We have the ultimate supplement, diet and workout regime for you to follow this summer.",
                    ImagePathLarge = "/Content/Images/Plans/female-plan2.jpg",
                    ImagePathMedium = "/Content/Images/Plans/female-plan2.jpg",
                    Name = "Bikini Body",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 4,
                            ItemType = ItemType.Plan,
                            Price = 20.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 6,
                            ItemType = ItemType.Plan,
                            Price = 30.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 27.99,
                        },
                    },
                };
                var plan7 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Your Transformation Plan!",
                    CreatedDate = DateTime.UtcNow,
                    Description = "My very own LiveFit fitness plan, which will help you lose weight, build shapely muscle, and get fit for life!",
                    ImagePathLarge = "/Content/Images/Plans/female-plan3.jpg",
                    ImagePathMedium = "/Content/Images/Plans/female-plan3.jpg",
                    Name = "Live Fit",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 4,
                            ItemType = ItemType.Plan,
                            Price = 20.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 6,
                            ItemType = ItemType.Plan,
                            Price = 30.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 27.99,
                        },
                    },
                };
                var plan8 = new Plan()
                {
                    Active = true,
                    BannerHeader = "The Do-Anywhere CrossFit Workout",
                    CreatedDate = DateTime.UtcNow,
                    Description = "No time? No equipment? No space? No problem. This travel-friendly, CrossFit-inspired workout will blast fat and sculpt muscle in minutes. No excuses!",
                    ImagePathLarge = "/Content/Images/Plans/female-plan4.jpg",
                    ImagePathMedium = "/Content/Images/Plans/female-plan4.jpg",
                    Name = "Crossfit Crazy",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 4,
                            ItemType = ItemType.Plan,
                            Price = 20.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 6,
                            ItemType = ItemType.Plan,
                            Price = 30.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 27.99,
                        },
                    },
                };
                
                context.Plans.Add(plan1);
                context.Plans.Add(plan2);
                context.Plans.Add(plan3);
                context.Plans.Add(plan4);
                context.Plans.Add(plan5);
                context.Plans.Add(plan6);
                context.Plans.Add(plan7);
                context.Plans.Add(plan8);
                context.SaveChanges();
            }

            if (!context.ImageConfigurations.Any())
            {
                context.ImageConfigurations.Add(new ImageConfiguration()
                {
                    Randomize = true,
                });
                context.SaveChanges();
        }

            if (!context.Roles.Any())
            {
                var userRole = new AuthRole()
                {
                    Name = JsfRoles.User
                };

                var adminRole = new AuthRole()
                {
                    Name = JsfRoles.Admin
                };
                
                context.Roles.Add(userRole);
                context.Roles.Add(adminRole);
                context.SaveChanges();
            }
            
            base.Seed(context);
        }
    }
}
