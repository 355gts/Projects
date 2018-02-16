using System;

namespace JoelScottFitness.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string DateTimeDisplayStringLong(this DateTime date)
        {
            string suffix = "th";
            string dayString = date.Day.ToString();

            if (date.Day < 11 || date.Day > 20)
            {
                dayString = dayString.ToCharArray()[dayString.ToCharArray().Length - 1].ToString();
                switch (dayString)
                {
                    case "1":
                        suffix = "st";
                        break;
                    case "2":
                        suffix = "nd";
                        break;
                    case "3":
                        suffix = "rd";
                        break;
                }
            }

            return string.Format(date.ToString("d{0} MMMM yyyy"), suffix);
        }
    }
}
