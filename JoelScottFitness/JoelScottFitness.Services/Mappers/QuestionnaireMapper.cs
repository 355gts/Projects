using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class QuestionnaireMapper : ITypeMapper<CreateQuestionnaireViewModel, Questionnaire>, ITypeMapper<Questionnaire, QuestionnaireViewModel>
    {
        public Questionnaire Map(CreateQuestionnaireViewModel fromObject, Questionnaire toObject = null)
        {
            var questionnaire = toObject ?? new Questionnaire();

            questionnaire.Age = fromObject.Age;
            questionnaire.IsCompleted = true;
            questionnaire.CurrentGym = fromObject.CurrentGym;
            questionnaire.WorkoutTypeId = fromObject.WorkoutTypeId;
            questionnaire.DietTypeId = fromObject.DietTypeId;
            questionnaire.DietDetails = fromObject.DietDetails;
            questionnaire.Height = fromObject.Height;
            questionnaire.IsMemberOfGym = fromObject.IsMemberOfGym;
            questionnaire.PurchaseId = fromObject.PurchaseId;
            questionnaire.TrainingGoals = fromObject.TrainingGoals;
            questionnaire.Weight = fromObject.Weight;
            questionnaire.WorkoutDescription = fromObject.WorkoutDescription;

            return questionnaire;
        }

        public QuestionnaireViewModel Map(Questionnaire fromObject, QuestionnaireViewModel toObject = null)
        {
            var questionnaire = toObject ?? new QuestionnaireViewModel();

            questionnaire.Age = fromObject.Age;
            questionnaire.CurrentGym = fromObject.CurrentGym;
            questionnaire.WorkoutTypeId = fromObject.WorkoutTypeId;
            questionnaire.DietTypeId = fromObject.DietTypeId;
            questionnaire.DietDetails = fromObject.DietDetails;
            questionnaire.Height = fromObject.Height;
            questionnaire.Id = fromObject.Id;
            questionnaire.IsMemberOfGym = fromObject.IsMemberOfGym;
            questionnaire.PurchaseId = fromObject.PurchaseId;
            questionnaire.TrainingGoals = fromObject.TrainingGoals;
            questionnaire.Weight = fromObject.Weight;
            questionnaire.WorkoutDescription = fromObject.WorkoutDescription;

            return questionnaire;
        }
    }
}
