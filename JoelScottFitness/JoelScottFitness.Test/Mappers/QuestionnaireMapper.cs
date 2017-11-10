using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class QuestionnaireMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new Questionnaire()
                {
                    Id = 123,
                    Age = 12,
                    CurrentGym = "CurrentGym",
                    WorkoutTypeId = 3,
                    DietDetails = "DietDetails",
                    DietTypeId = 3,
                    Height = 97,
                    IsMemberOfGym = true,
                    TrainingGoals = "TrainingGoals",
                    Weight = 345,
                    WorkoutDescription = "WorkoutDescription",
                };

                var mapper = new Map.QuestionnaireMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new Questionnaire()
                {
                    Id = 123,
                    Age = 12,
                    CurrentGym = "CurrentGym",
                    WorkoutTypeId = 3,
                    DietDetails = "DietDetails",
                    DietTypeId = 3,
                    Height = 97,
                    IsMemberOfGym = true,
                    TrainingGoals = "TrainingGoals",
                    Weight = 345,
                    WorkoutDescription = "WorkoutDescription",
                };

                QuestionnaireViewModel toObject = new QuestionnaireViewModel();

                var mapper = new Map.QuestionnaireMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Questionnaire repoObject, QuestionnaireViewModel webObject)
            {
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.Age.ToString(), webObject.Age);
                Assert.AreEqual(repoObject.CurrentGym, webObject.CurrentGym);
                Assert.AreEqual(repoObject.WorkoutTypeId, webObject.WorkoutTypeId);
                Assert.AreEqual(repoObject.DietDetails, webObject.DietDetails);
                Assert.AreEqual(repoObject.DietTypeId, webObject.DietTypeId);
                Assert.AreEqual(repoObject.Height.ToString(), webObject.Height);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.IsMemberOfGym, webObject.IsMemberOfGym);
                Assert.AreEqual(repoObject.TrainingGoals, webObject.TrainingGoals);
                Assert.AreEqual(repoObject.Weight.ToString(), webObject.Weight);
                Assert.AreEqual(repoObject.WorkoutDescription, webObject.WorkoutDescription);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new QuestionnaireViewModel()
                {
                    Id = 123,
                    Age = "12",
                    CurrentGym = "CurrentGym",
                    WorkoutTypeId = 3,
                    DietDetails = "DietDetails",
                    DietTypeId = 3,
                    Height = "97",
                    IsMemberOfGym = true,
                    PurchaseId = 456,
                    TrainingGoals = "TrainingGoals",
                    Weight = "345",
                    WorkoutDescription = "WorkoutDescription",
                };

                var mapper = new Map.QuestionnaireMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new QuestionnaireViewModel()
                {
                    Id = 123,
                    Age = "12",
                    CurrentGym = "CurrentGym",
                    WorkoutTypeId = 3,
                    DietDetails = "DietDetails",
                    DietTypeId = 3,
                    Height = "97",
                    IsMemberOfGym = true,
                    PurchaseId = 456,
                    TrainingGoals = "TrainingGoals",
                    Weight = "345",
                    WorkoutDescription = "WorkoutDescription",
                };

                Questionnaire toObject = new Questionnaire();

                var mapper = new Map.QuestionnaireMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(QuestionnaireViewModel repoObject, Questionnaire webObject)
            {
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.Age.ToString(), repoObject.Age);
                Assert.AreEqual(webObject.CurrentGym, repoObject.CurrentGym);
                Assert.AreEqual(webObject.WorkoutTypeId, repoObject.WorkoutTypeId);
                Assert.AreEqual(webObject.DietDetails, repoObject.DietDetails);
                Assert.AreEqual(webObject.DietTypeId, repoObject.DietTypeId);
                Assert.AreEqual(webObject.Height.ToString(), repoObject.Height);
                Assert.AreEqual(webObject.IsMemberOfGym, repoObject.IsMemberOfGym);
                Assert.AreEqual(webObject.TrainingGoals, repoObject.TrainingGoals);
                Assert.AreEqual(webObject.Weight.ToString(), repoObject.Weight);
                Assert.AreEqual(webObject.WorkoutDescription, repoObject.WorkoutDescription);
            }
        }
    }
}
