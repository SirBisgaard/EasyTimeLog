using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MEB.EasyTimeLog.Domain;

namespace MEB.EasyTimeLog.Model
{
    public class TimeUtil
    {
        public const string TimeSpanFormat = @"hh\:mm";
        public const string TimeSpanHourFormat = @"h\:mm";

        public const string DateMonthFormat = "yyyy - MM";

        public const string DefaultTimeEntityFormat = "{0}: {2} - {3} Duration={4}";

        public static Calendar Calendar => DateTimeFormatInfo.CurrentInfo.Calendar;

        public static bool Conflict(LogEntity entity1, LogEntity entity2)
        {
            if (entity1.Id == entity2.Id)
            {
                return false;
            }

            // Check if the day are equal.
            if (entity1.Day.CompareTo(entity2.Day) == 0)
            {
                // Check if the time conflicts.
                if ((
                    // Check if the from time is inside.
                    entity1.TimeFrom.CompareTo(entity2.TimeFrom) == -1 &&
                    entity1.TimeTo.CompareTo(entity2.TimeFrom) == 1) || (
                    // Check if the to time is inside.
                    entity1.TimeFrom.CompareTo(entity2.TimeTo) == -1 &&
                    entity1.TimeTo.CompareTo(entity2.TimeTo) == 1) || (
                    // Check if the to time is the same.
                    entity1.TimeFrom.CompareTo(entity2.TimeFrom) == 0 &&
                    entity1.TimeTo.CompareTo(entity2.TimeTo) == 0) || (
                    // Check if the time is over lapping.
                    entity1.TimeFrom.CompareTo(entity2.TimeFrom) == 1 &&
                    entity1.TimeTo.CompareTo(entity2.TimeTo) == -1))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetDuration(IEnumerable<LogEntity> entities)
        {
            if(entities == null)
            {
                return "0";
            }

            var hours = 0d;

            foreach (var entity in entities)
            {
                // Get the duration and return it in a nice format.

                hours += entity.GetDuration();
            }

            return hours.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetDuration(TimeSpan from, TimeSpan to)
        {
            // Get the duration and return it in a nice format.
            return from.Subtract(to).ToString(TimeSpanHourFormat);
        }

        public static List<string> GetListOfAvalibleDays(IEnumerable<LogEntity> entities)
        {
            // Create a set of dates for only getting one of each day.
            var dates = new SortedSet<DateTime>();
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Add the entry days to the set.
            entities.ToList().ForEach(entry => dates.Add(entry.Day));

            // Convert all entities in the set to string.
            foreach (var date in dates)
            {
                dateStrings.Add(date.ToString("yyyy - MM - dd"));
            }

            return dateStrings;
        }

        public static IList<string> GetListOfAvalibleWeeks(IEnumerable<LogEntity> entities)
        {
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Convert all entities in the set to string.
            foreach (var e in entities)
            {
                var s = $"{e.Day.Year} - Week {Calendar.GetWeekOfYear(e.Day, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}";
                if (!dateStrings.Contains(s))
                {
                    dateStrings.Add(s);
                }
            }

            dateStrings.Sort();

            return dateStrings;
        }

        public static IList<string> GetListOfAvalibleYear(IEnumerable<LogEntity> entities)
        {
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Convert all entities in the set to string.
            foreach (var e in entities)
            {
                var s = e.Day.Year.ToString();
                if (!dateStrings.Contains(s))
                {
                    dateStrings.Add(s);
                }
            }

            dateStrings.Sort();

            return dateStrings;
        }

        public static IList<string> GetListOfAvalibleMonths(IEnumerable<LogEntity> entities)
        {
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Convert all entities in the set to string.
            foreach (var e in entities)
            {
                var s = e.Day.ToString("yyyy - MM");
                if (!dateStrings.Contains(s))
                {
                    dateStrings.Add(s);
                }
            }

            dateStrings.Sort();

            return dateStrings;
        }
    }
}
