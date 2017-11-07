using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreateBlogImageMapper
    {
        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new CreateBlogImageViewModel()
                {
                    Caption = "Caption",
                    CaptionColour = BlogCaptionTextColour.White,
                    CaptionTitle = "CaptionTitle",
                    ImagePath = "ImagePath",
                };

                var mapper = new Map.CreateBlogImageMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CreateBlogImageViewModel()
                {
                    Caption = "Caption",
                    CaptionColour = BlogCaptionTextColour.White,
                    CaptionTitle = "CaptionTitle",
                    ImagePath = "ImagePath",
                };

                BlogImage toObject = new BlogImage();

                var mapper = new Map.CreateBlogImageMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CreateBlogImageViewModel webObject, BlogImage repoObject)
            {
                Assert.AreEqual(webObject.Caption, repoObject.Caption);
                Assert.AreEqual(webObject.CaptionColour, repoObject.CaptionColour);
                Assert.AreEqual(webObject.CaptionTitle, repoObject.CaptionTitle);
                Assert.AreEqual(webObject.ImagePath, repoObject.ImagePath);
            }
        }
    }
}
