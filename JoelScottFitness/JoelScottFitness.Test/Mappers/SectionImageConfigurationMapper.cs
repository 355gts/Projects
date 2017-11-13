using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class SectionImageConfigurationMapper
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

                var mapper = new Map.SectionImageConfigurationMapper();

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

                SectionImageViewModel webOject = new SectionImageViewModel();

                var mapper = new Map.SectionImageConfigurationMapper();

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

                var mapper = new Map.SectionImageConfigurationMapper();

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

                SectionImageViewModel webOject = new SectionImageViewModel();

                var mapper = new Map.SectionImageConfigurationMapper();

                mapper.Map(repoObject, webOject);

                AssertAreEqual(repoObject, webOject, false);
            }

            private void AssertAreEqual(ImageConfiguration repoObject, SectionImageViewModel webObject, bool random)
            {
                if (random)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(webObject.SectionImage1));
                    Assert.IsTrue(!string.IsNullOrEmpty(webObject.SectionImage2));
                    Assert.IsTrue(!string.IsNullOrEmpty(webObject.SectionImage3));
                    Assert.IsTrue(!string.IsNullOrEmpty(webObject.SplashImage));
                }
                else
                {
                    Assert.IsTrue(webObject.SectionImage1.Contains("SectionImage1Id"));
                    Assert.IsTrue(webObject.SectionImage2.Contains("SectionImage2Id"));
                    Assert.IsTrue(webObject.SectionImage3.Contains("SectionImage3Id"));
                    Assert.IsTrue(webObject.SplashImage.Contains("SplashImageId"));
                }
            }
        }
    }
}
