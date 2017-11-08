using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreateBlogMapper
    {
        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new CreateBlogViewModel()
                {
                    Content = "Content",
                    ImagePath = "ImagePath",
                    SubHeader = "SubHeader",
                    Title = "Title",
                    BlogImages = new List<CreateBlogImageViewModel>()
                    {
                        new CreateBlogImageViewModel()
                        {
                            Caption = "Caption",
                            CaptionColour = BlogCaptionTextColour.Black,
                            CaptionTitle = "CaptionTitle",
                            ImagePath = "ImagePath",
                        },
                    },
                };

                var mapper = new Map.CreateBlogMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CreateBlogViewModel()
                {
                    Content = "Content",
                    ImagePath = "ImagePath",
                    SubHeader = "SubHeader",
                    Title = "Title",
                    BlogImages = new List<CreateBlogImageViewModel>()
                    {
                        new CreateBlogImageViewModel()
                        {
                            Caption = "Caption",
                            CaptionColour = BlogCaptionTextColour.Black,
                            CaptionTitle = "CaptionTitle",
                            ImagePath = "ImagePath",
                        },
                    },
                };

                Blog toObject = new Blog();

                var mapper = new Map.CreateBlogMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CreateBlogViewModel webObject, Blog repoObject)
            {
                Assert.AreEqual(webObject.Content, repoObject.Content);
                Assert.IsNotNull(repoObject.CreatedDate);
                Assert.IsNull(repoObject.ModifiedDate);
                Assert.AreEqual(0, repoObject.Id);
                Assert.AreEqual(webObject.ImagePath, repoObject.ImagePath);
                Assert.AreEqual(webObject.SubHeader, repoObject.SubHeader);
                Assert.AreEqual(webObject.Title, repoObject.Title);
                Assert.IsFalse(repoObject.Active);

                Assert.IsNotNull(repoObject.BlogImages);
                Assert.AreEqual(1, repoObject.BlogImages.Count());

                var blogImage = repoObject.BlogImages.First();

                Assert.AreEqual(0, blogImage.BlogId);
                Assert.AreEqual("Caption", blogImage.Caption);
                Assert.AreEqual(BlogCaptionTextColour.Black, blogImage.CaptionColour);
                Assert.AreEqual("CaptionTitle", blogImage.CaptionTitle);
                Assert.AreEqual(0, blogImage.Id);
                Assert.AreEqual("ImagePath", blogImage.ImagePath);
            }
        }
    }
}
