using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class ImageConfigurationMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject_Random()
            {
                var repoObject = new ImageConfiguration()
                {
                    Randomize = true,
                    SectionImage1Id = 111,
                    SectionImage2Id = 222,
                    SectionImage3Id = 333,
                    SplashImageId = 444,
                    Images = new List<Image>()
                    {
                        new Image(){ ImagePath = "RandomImage1" },
                        new Image(){ ImagePath = "RandomImage2" },
                        new Image(){ ImagePath = "RandomImage3" },
                        new Image(){ ImagePath = "RandomImage4" },
                        new Image(){ ImagePath = "RandomImage5" },
                    },
                    SectionImage1 = new Image() { ImagePath = "SectionImage1" },
                    SectionImage2 = new Image() { ImagePath = "SectionImage2" },
                    SectionImage3 = new Image() { ImagePath = "SectionImage3" },
                    SplashImage = new Image() { ImagePath = "SplashImage" },
                };

                var mapper = new Map.ImageConfigurationMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result, true);
            }

            [TestMethod]
            public void FromObject_ToExistingObject_Random()
            {
                var repoObject = new ImageConfiguration()
                {
                    Randomize = true,
                    SectionImage1Id = 111,
                    SectionImage2Id = 222,
                    SectionImage3Id = 333,
                    SplashImageId = 444,
                    Images = new List<Image>()
                    {
                        new Image(){ ImagePath = "RandomImage1" },
                        new Image(){ ImagePath = "RandomImage2" },
                        new Image(){ ImagePath = "RandomImage3" },
                        new Image(){ ImagePath = "RandomImage4" },
                        new Image(){ ImagePath = "RandomImage5" },
                    },
                    SectionImage1 = new Image() { ImagePath = "SectionImage1" },
                    SectionImage2 = new Image() { ImagePath = "SectionImage2" },
                    SectionImage3 = new Image() { ImagePath = "SectionImage3" },
                    SplashImage = new Image() { ImagePath = "SplashImage" },
                };

                SectionImageViewModel webOject = new SectionImageViewModel();

                var mapper = new Map.ImageConfigurationMapper();

                mapper.Map(repoObject, webOject);

                AssertAreEqual(repoObject, webOject, true);
            }


            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new ImageConfiguration()
                {
                    Randomize = false,
                    SectionImage1Id = 111,
                    SectionImage2Id = 222,
                    SectionImage3Id = 333,
                    SplashImageId = 444,
                    Images = new List<Image>()
                    {
                        new Image(){ ImagePath = "RandomImage1" },
                        new Image(){ ImagePath = "RandomImage2" },
                        new Image(){ ImagePath = "RandomImage3" },
                        new Image(){ ImagePath = "RandomImage4" },
                        new Image(){ ImagePath = "RandomImage5" },
                    },
                    SectionImage1 = new Image() { ImagePath = "SectionImage1" },
                    SectionImage2 = new Image() { ImagePath = "SectionImage2" },
                    SectionImage3 = new Image() { ImagePath = "SectionImage3" },
                    SplashImage = new Image() { ImagePath = "SplashImage" },
                };

                var mapper = new Map.ImageConfigurationMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result, false);
            }

            [TestMethod]
            public void FromObject_ToExistingObject()
            {
                var repoObject = new ImageConfiguration()
                {
                    Randomize = false,
                    SectionImage1Id = 111,
                    SectionImage2Id = 222,
                    SectionImage3Id = 333,
                    SplashImageId = 444,
                    Images = new List<Image>()
                    {
                        new Image(){ ImagePath = "RandomImage1" },
                        new Image(){ ImagePath = "RandomImage2" },
                        new Image(){ ImagePath = "RandomImage3" },
                        new Image(){ ImagePath = "RandomImage4" },
                        new Image(){ ImagePath = "RandomImage5" },
                    },
                    SectionImage1 = new Image() { ImagePath = "SectionImage1" },
                    SectionImage2 = new Image() { ImagePath = "SectionImage2" },
                    SectionImage3 = new Image() { ImagePath = "SectionImage3" },
                    SplashImage = new Image() { ImagePath = "SplashImage" },
                };

                SectionImageViewModel webOject = new SectionImageViewModel();

                var mapper = new Map.ImageConfigurationMapper();

                mapper.Map(repoObject, webOject);

                AssertAreEqual(repoObject, webOject, false);
            }

            private void AssertAreEqual(ImageConfiguration repoObject, SectionImageViewModel webObject, bool random)
            {
                if (random)
                {
                    Assert.IsTrue(webObject.SectionImage1.Contains("Random"));
                    Assert.IsTrue(webObject.SectionImage2.Contains("Random"));
                    Assert.IsTrue(webObject.SectionImage3.Contains("Random"));
                    Assert.IsTrue(webObject.SplashImage.Contains("Random"));
                }
                else
                {
                    Assert.AreEqual(repoObject.SectionImage1.ImagePath, webObject.SectionImage1);
                    Assert.AreEqual(repoObject.SectionImage2.ImagePath, webObject.SectionImage2);
                    Assert.AreEqual(repoObject.SectionImage3.ImagePath, webObject.SectionImage3);
                    Assert.AreEqual(repoObject.SplashImage.ImagePath, webObject.SplashImage);
                }
            }
        }
        
        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new ImageConfigurationViewModel()
                {
                    Randomize = true,
                    SectionImage1Id = 111,
                    SectionImage2Id = 222,
                    SectionImage3Id = 333,
                    SplashImageId = 444,
                };

                var mapper = new Map.ImageConfigurationMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new ImageConfigurationViewModel()
                {
                    Randomize = true,
                    SectionImage1Id = 111,
                    SectionImage2Id = 222,
                    SectionImage3Id = 333,
                    SplashImageId = 444,
                };

                var repoObject = new ImageConfiguration();

                var mapper = new Map.ImageConfigurationMapper();

                mapper.Map(webObject, repoObject);

                AssertAreEqual(webObject, repoObject);
            }

            private void AssertAreEqual(ImageConfigurationViewModel webObject, ImageConfiguration repoObject)
            {
                Assert.AreEqual(webObject.Randomize, repoObject.Randomize);
                Assert.AreEqual(webObject.SectionImage1Id, repoObject.SectionImage1Id);
                Assert.AreEqual(webObject.SectionImage2Id, repoObject.SectionImage2Id);
                Assert.AreEqual(webObject.SectionImage3Id, repoObject.SectionImage3Id);
                Assert.AreEqual(webObject.SplashImageId, repoObject.SplashImageId);
            }
        }
    }
}
