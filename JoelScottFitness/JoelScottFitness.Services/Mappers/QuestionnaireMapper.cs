using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;

namespace JoelScottFitness.Services.Mappers
{
    sealed class QuestionnaireMapper : ITypeMapper<QuestionnaireViewModel, Questionnaire>, ITypeMapper<Questionnaire, QuestionnaireViewModel>
    {
        public Questionnaire Map(QuestionnaireViewModel fromObject, Questionnaire toObject = null)
        {
            var questionnaire = toObject ?? new Questionnaire();

            questionnaire.Id = fromObject.Id;
            questionnaire.Age = Convert.ToInt32(fromObject.Age);
            questionnaire.CurrentGym = fromObject.CurrentGym;
            questionnaire.WorkoutTypeId = fromObject.WorkoutTypeId;
            questionnaire.DietTypeId = fromObject.DietTypeId;
            questionnaire.DietDetails = fromObject.DietDetails;
            questionnaire.Height = Convert.ToInt32(fromObject.Height);
            questionnaire.IsMemberOfGym = fromObject.IsMemberOfGym;
            questionnaire.PurchaseId = fromObject.PurchaseId;
            questionnaire.TrainingGoals = fromObject.TrainingGoals;
            questionnaire.Weight = Convert.ToInt32(fromObject.Weight);
            questionnaire.WorkoutDescription = fromObject.WorkoutDescription;

            return questionnaire;
        }

        public QuestionnaireViewModel Map(Questionnaire fromObject, QuestionnaireViewModel toObject = null)
        {
            var questionnaire = toObject ?? new QuestionnaireViewModel();

            questionnaire.Id = fromObject.Id;
            questionnaire.Age = fromObject.Age.ToString();
            questionnaire.CurrentGym = fromObject.CurrentGym;
            questionnaire.WorkoutTypeId = fromObject.WorkoutTypeId;
            questionnaire.DietTypeId = fromObject.DietTypeId;
            questionnaire.DietDetails = fromObject.DietDetails;
            questionnaire.Height = fromObject.Height.ToString();
            questionnaire.Id = fromObject.Id;
            questionnaire.IsMemberOfGym = fromObject.IsMemberOfGym;
            questionnaire.PurchaseId = fromObject.PurchaseId;
            questionnaire.TrainingGoals = fromObject.TrainingGoals;
            questionnaire.Weight = fromObject.Weight.ToString();
            questionnaire.WorkoutDescription = fromObject.WorkoutDescription;

            return questionnaire;
        }
    }
}
