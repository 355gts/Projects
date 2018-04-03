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
                            Name = "Shortcut to Shred",
                            Description = "4 Week Shred",
                            Duration = 4,
                            ImagePath = "/Content/Images/Plans/ShortcutToShred.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 19.99,
                            CreatedDate = DateTime.UtcNow,
                        },

                        new PlanOption()
                        {
                            Name = "Shortcut to Shred",
                            Description = "8 Week Shred",
                            Duration = 8,
                            ImagePath = "/Content/Images/Plans/ShortcutToShred.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 29.99,
                            CreatedDate = DateTime.UtcNow,
                        },

                        new PlanOption()
                        {
                            Name = "Shortcut to Shred",
                            Description = "12 Week Shred",
                            Duration = 12,
                            ImagePath = "/Content/Images/Plans/ShortcutToShred.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 39.99,
                            CreatedDate = DateTime.UtcNow,
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
                            Name = "The Hulk Effect",
                            Description = "4 Week Plan",
                            Duration = 4,
                            ImagePath = "/Content/Images/Plans/TheHulkEffect.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 19.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "The Hulk Effect",
                            Description = "8 Week Plan",
                            Duration = 8,
                            ImagePath = "/Content/Images/Plans/TheHulkEffect.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 29.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "The Hulk Effect",
                            Description = "12 Week Plan",
                            Duration = 12,
                            ImagePath = "/Content/Images/Plans/TheHulkEffect.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 39.99,
                            CreatedDate = DateTime.UtcNow,
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
                            Name = "Project Mass",
                            Description = "4 Week Plan",
                            Duration = 4,
                            ImagePath = "/Content/Images/Plans/ProjectMass.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 19.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "Project Mass",
                            Description = "8 Week Plan",
                            Duration = 8,
                            ImagePath = "/Content/Images/Plans/ProjectMass.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 29.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "Project Mass",
                            Description = "12 Week Plan",
                            Duration = 12,
                            ImagePath = "/Content/Images/Plans/ProjectMass.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 39.99,
                            CreatedDate = DateTime.UtcNow,
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
                            Name = "Wonder Woman Physique",
                            Description = "4 Week Plan",
                            Duration = 14,
                            ImagePath = "/Content/Images/Plans/WonderWomanPhysique.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 19.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "Wonder Woman Physique",
                            Description = "8 Week Plan",
                            Duration = 8,
                            ImagePath = "/Content/Images/Plans/WonderWomanPhysique.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 29.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "Wonder Woman Physique",
                            Description = "12 Week Plan",
                            Duration = 12,
                            ImagePath = "/Content/Images/Plans/WonderWomanPhysique.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 39.99,
                            CreatedDate = DateTime.UtcNow,
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
                            Name = "Ultimate Booty Builder",
                            Description = "4 Week Plan",
                            Duration = 14,
                            ImagePath = "/Content/Images/Plans/UltimateBootyBuilder.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 19.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "Ultimate Booty Builder",
                            Description = "8 Week Plan",
                            Duration = 8,
                            ImagePath = "/Content/Images/Plans/UltimateBootyBuilder.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 29.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "Ultimate Booty Builder",
                            Description = "12 Week Plan",
                            Duration = 12,
                            ImagePath = "/Content/Images/Plans/UltimateBootyBuilder.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 39.99,
                            CreatedDate = DateTime.UtcNow,
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
                            Name = "Beach Body",
                            Description = "4 Week Plan",
                            Duration = 14,
                            ImagePath = "/Content/Images/Plans/BeachBody.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 19.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "Beach Body",
                            Description = "8 Week Plan",
                            Duration = 8,
                            ImagePath = "/Content/Images/Plans/BeachBody.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 29.99,
                            CreatedDate = DateTime.UtcNow,
                        },
                        new PlanOption()
                        {
                            Name = "Beach Body",
                            Description = "12 Week Plan",
                            Duration = 12,
                            ImagePath = "/Content/Images/Plans/BeachBody.jpg",
                            ItemCategory = ItemCategory.Plan,
                            Price = 39.99,
                            CreatedDate = DateTime.UtcNow,
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

            if (!context.Images.Any())
            {
                var image1 = new Image() { ImagePath = "/Content/Images/BGZD3738.JPG" };
                var image2 = new Image() { ImagePath = "/Content/Images/BXJA2014.JPG" };
                var image3 = new Image() { ImagePath = "/Content/Images/CZOB9407.JPG" };
                var image4 = new Image() { ImagePath = "/Content/Images/FIOI1024.JPG" };
                var image5 = new Image() { ImagePath = "/Content/Images/IMG_00001.JPG" };
                var image6 = new Image() { ImagePath = "/Content/Images/IMG_00002.JPG" };
                var image7 = new Image() { ImagePath = "/Content/Images/IMG_00003.JPG" };
                var image8 = new Image() { ImagePath = "/Content/Images/IMG_00004.JPG" };
                var image9 = new Image() { ImagePath = "/Content/Images/IMG_00005.JPG" };
                var image10 = new Image() { ImagePath = "/Content/Images/IMG_00006.JPG" };
                var image11 = new Image() { ImagePath = "/Content/Images/IMG_00007.JPG" };
                var image12 = new Image() { ImagePath = "/Content/Images/IMG_00008.JPG" };
                var image13 = new Image() { ImagePath = "/Content/Images/IMG_00010.JPG" };
                var image14 = new Image() { ImagePath = "/Content/Images/IMG_00011.JPG" };
                var image15 = new Image() { ImagePath = "/Content/Images/IMG_00012.JPG" };
                var image16 = new Image() { ImagePath = "/Content/Images/IMG_00013.JPG" };
                var image17 = new Image() { ImagePath = "/Content/Images/IMG_00014.JPG" };
                var image18 = new Image() { ImagePath = "/Content/Images/IMG_00015.JPG" };
                var image19 = new Image() { ImagePath = "/Content/Images/IMG_00016.JPG" };
                var image20 = new Image() { ImagePath = "/Content/Images/IMG_00017.JPG" };
                var image21 = new Image() { ImagePath = "/Content/Images/IMG_00018.JPG" };
                var image22 = new Image() { ImagePath = "/Content/Images/IMG_00019.JPG" };
                var image23 = new Image() { ImagePath = "/Content/Images/IMG_00020.JPG" };
                var image24 = new Image() { ImagePath = "/Content/Images/IMG_00021.JPG" };
                var image25 = new Image() { ImagePath = "/Content/Images/IMG_6173.JPG" };
                var image26 = new Image() { ImagePath = "/Content/Images/IMG_6313.jpg" };
                var image27 = new Image() { ImagePath = "/Content/Images/UWBL7758.JPG" };

                context.Images.Add(image1);
                context.Images.Add(image2);
                context.Images.Add(image3);
                context.Images.Add(image4);
                context.Images.Add(image5);
                context.Images.Add(image6);
                context.Images.Add(image7);
                context.Images.Add(image8);
                context.Images.Add(image9);
                context.Images.Add(image10);
                context.Images.Add(image11);
                context.Images.Add(image12);
                context.Images.Add(image13);
                context.Images.Add(image14);
                context.Images.Add(image15);
                context.Images.Add(image16);
                context.Images.Add(image17);
                context.Images.Add(image18);
                context.Images.Add(image19);
                context.Images.Add(image20);
                context.Images.Add(image21);
                context.Images.Add(image22);
                context.Images.Add(image23);
                context.Images.Add(image24);
                context.Images.Add(image25);
                context.Images.Add(image26);
                context.Images.Add(image27);
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
