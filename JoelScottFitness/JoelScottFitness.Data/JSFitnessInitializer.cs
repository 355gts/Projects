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
                    BannerHeader = "Get Shredded!",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Get ready to burn fat, boost confidence and get in the best shape of your life! With as little cardio as possible and an enjoyable diet, get ready for.. The Shortcut to Shred!",
                    ImagePathLarge = "/Content/Images/Plans/ShortcutToShred.jpg",
                    Name = "Shortcut to Shred",
                    TargetGender = Gender.Male,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Shred",
                            Duration = 4,
                            ItemType = ItemType.Plan,
                            Price = 19.99,
                        },

                        new PlanOption()
                        {
                            Description = "8 Week Shred",
                            Duration = 8,
                            ItemType = ItemType.Plan,
                            Price = 29.99,
                        },

                        new PlanOption()
                        {
                            Description = "12 Week Shred",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 39.99,
                        },
                    },
                };
                var plan2 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Get Shredded!",
                    CreatedDate = DateTime.UtcNow,
                    Description = "You want it all! Solid muscle, with impressive strength to back it up. Combining compound strength training with a bodybuilding approach.. GET BIGGER, GET STRONGER!",
                    ImagePathLarge = "/Content/Images/Plans/TheHulkEffect.jpg",
                    Name = "The Hulk Effect",
                    TargetGender = Gender.Male,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 4,
                            ItemType = ItemType.Plan,
                            Price = 19.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 8,
                            ItemType = ItemType.Plan,
                            Price = 29.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 39.99,
                        },
                    },
                };
                var plan3 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Muscle Building Blueprint",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Project Mass is a next level training, nutrition, and supplement program, designed to build as much muscle as possible, in the shortest space of time.. This is how you grow!",
                    ImagePathLarge = "/Content/Images/Plans/ProjectMass.jpg",
                    Name = "Project Mass",
                    TargetGender = Gender.Male,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 4,
                            ItemType = ItemType.Plan,
                            Price = 19.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 8,
                            ItemType = ItemType.Plan,
                            Price = 29.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 39.99,
                        },
                    },
                };
                var plan4 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Shape & Strength Shortcut",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Shape up, gain strength, gain confidence.. Look wonderful!",
                    ImagePathLarge = "/Content/Images/Plans/WonderWomanPhysique.jpg",
                    Name = "Wonder Woman Physique",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 14,
                            ItemType = ItemType.Plan,
                            Price = 19.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 8,
                            ItemType = ItemType.Plan,
                            Price = 29.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 39.99,
                        },
                    },
                };
                var plan5 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Booty Building Trainer",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Specified training to get the best results in the shortest space of time.. Get ready to blow up that butt!",
                    ImagePathLarge = "/Content/Images/Plans/UltimateBootyBuilder.jpg",
                    Name = "Ultimate Booty Builder",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 14,
                            ItemType = ItemType.Plan,
                            Price = 19.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 8,
                            ItemType = ItemType.Plan,
                            Price = 29.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 39.99,
                        },
                    },
                };
                var plan6 = new Plan()
                {
                    Active = true,
                    BannerHeader = "Bikini ready, in no time!",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Holiday booked? Summer on the way? Dust off those bikinis and get the body you've always wanted!",
                    ImagePathLarge = "/Content/Images/Plans/BeachBody.jpg",
                    Name = "Beach Body",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>()
                    {
                        new PlanOption()
                        {
                            Description = "4 Week Plan",
                            Duration = 14,
                            ItemType = ItemType.Plan,
                            Price = 19.99,
                        },
                        new PlanOption()
                        {
                            Description = "8 Week Plan",
                            Duration = 8,
                            ItemType = ItemType.Plan,
                            Price = 29.99,
                        },
                        new PlanOption()
                        {
                            Description = "12 Week Plan",
                            Duration = 12,
                            ItemType = ItemType.Plan,
                            Price = 39.99,
                        },
                    },
                };

                context.Plans.Add(plan1);
                context.Plans.Add(plan2);
                context.Plans.Add(plan3);
                context.Plans.Add(plan4);
                context.Plans.Add(plan5);
                context.Plans.Add(plan6);
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
