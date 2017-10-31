using System.Collections.Generic;

namespace JoelScottFitness.Common.OptionLists
{
    public static class Options
    {
        public static List<KeyValuePair<int, string>> WorkoutTypes
        {
            get
            {
                return new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>(0, "Don't currently workout"),
                    new KeyValuePair<int, string>(1, "Few times a month"),
                    new KeyValuePair<int, string>(2, "Few times a week"),
                    new KeyValuePair<int, string>(3, "Workout every day"),
                    new KeyValuePair<int, string>(4, "Live in the gym"),
                };
            }
        }

        public static List<KeyValuePair<int, string>> DietTypes
        {
            get
            {
                return new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>(0, "I live on junk food"),
                    new KeyValuePair<int, string>(1, "Eat junk food frequently"),
                    new KeyValuePair<int, string>(2, "Eat healthy but cheat sometimes"),
                    new KeyValuePair<int, string>(3, "Only eat healthy food"),
                };
            }
        }

        public static List<KeyValuePair<bool, string>> TrueFalseTypes
        {
            get
            {
                return new List<KeyValuePair<bool, string>>()
                {
                    new KeyValuePair<bool, string>(false, "No"),
                    new KeyValuePair<bool, string>(true, "Yes"),
                };
            }
        }
    }
}
