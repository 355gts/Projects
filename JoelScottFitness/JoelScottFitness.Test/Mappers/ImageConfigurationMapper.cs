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
                        new Image(){ Id = 1, ImagePath = "RandomImage1" },
                        new Image(){ Id = 2, ImagePath = "RandomImage2" },
                        new Image(){ Id = 3, ImagePath = "RandomImage3" },
                        new Image(){ Id = 4, ImagePath = "RandomImage4" },
                        new Image(){ Id = 5, ImagePath = "RandomImage5" },
                        new Image(){ Id = 111 ,ImagePath = "SectionImage1Id" },
                        new Image(){ Id = 222 ,ImagePath = "SectionImage2Id" },
                        new Image(){ Id = 333 ,ImagePath = "SectionImage3Id" },
                        new Image(){ Id = 444 ,ImagePath = "SplashImageId" },
                    },
                };

                var mapper = new Map.ImageConfigurationMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
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
                        new Image(){ Id = 1, ImagePath = "RandomImage1" },
                        new Image(){ Id = 2, ImagePath = "RandomImage2" },
                        new Image(){ Id = 3, ImagePath = "RandomImage3" },
                        new Image(){ Id = 4, ImagePath = "RandomImage4" },
                        new Image(){ Id = 5, ImagePath = "RandomImage5" },
                        new Image(){ Id = 111 ,ImagePath = "SectionImage1Id" },
                        new Image(){ Id = 222 ,ImagePath = "SectionImage2Id" },
                        new Image(){ Id = 333 ,ImagePath = "SectionImage3Id" },
                        new Image(){ Id = 444 ,ImagePath = "SplashImageId" },
                    },
                };

                ImageConfigurationViewModel webOject = new ImageConfigurationViewModel();

                var mapper = new Map.ImageConfigurationMapper();

                mapper.Map(repoObject, webOject);

                AssertAreEqual(repoObject, webOject);
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
                        new Image(){ Id = 1, ImagePath = "RandomImage1" },
                        new Image(){ Id = 2, ImagePath = "RandomImage2" },
                        new Image(){ Id = 3, ImagePath = "RandomImage3" },
                        new Image(){ Id = 4, ImagePath = "RandomImage4" },
                        new Image(){ Id = 5, ImagePath = "RandomImage5" },
                        new Image(){ Id = 111 ,ImagePath = "SectionImage1Id" },
                        new Image(){ Id = 222 ,ImagePath = "SectionImage2Id" },
                        new Image(){ Id = 333 ,ImagePath = "SectionImage3Id" },
                        new Image(){ Id = 444 ,ImagePath = "SplashImageId" },
                    },
                };

                var mapper = new Map.ImageConfigurationMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
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
                        new Image(){ Id = 1, ImagePath = "RandomImage1" },
                        new Image(){ Id = 2, ImagePath = "RandomImage2" },
                        new Image(){ Id = 3, ImagePath = "RandomImage3" },
                        new Image(){ Id = 4, ImagePath = "RandomImage4" },
                        new Image(){ Id = 5, ImagePath = "RandomImage5" },
                        new Image(){ Id = 111 ,ImagePath = "SectionImage1Id" },
                        new Image(){ Id = 222 ,ImagePath = "SectionImage2Id" },
                        new Image(){ Id = 333 ,ImagePath = "SectionImage3Id" },
                        new Image(){ Id = 444 ,ImagePath = "SplashImageId" },
                    },
                };

                ImageConfigurationViewModel webOject = new ImageConfigurationViewModel();

                var mapper = new Map.ImageConfigurationMapper();

                mapper.Map(repoObject, webOject);

                AssertAreEqual(repoObject, webOject);
            }

            private void AssertAreEqual(ImageConfiguration repoObject, ImageConfigurationViewModel webObject)
            {
                Assert.AreEqual(repoObject.Randomize, webObject.Randomize);
                Assert.AreEqual(repoObject.SectionImage1Id, webObject.SectionImage1Id);
                Assert.AreEqual(repoObject.SectionImage2Id, webObject.SectionImage2Id);
                Assert.AreEqual(repoObject.SectionImage3Id, webObject.SectionImage3Id);
                Assert.AreEqual(repoObject.SplashImageId, webObject.SplashImageId);
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
