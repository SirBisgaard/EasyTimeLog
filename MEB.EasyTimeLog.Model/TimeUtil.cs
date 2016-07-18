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

        public const string DefaultTimeEntryFormat = "{0}: {2} - {3} Duration={4}";

        public static Calendar Calendar => DateTimeFormatInfo.CurrentInfo.Calendar;

        public static bool Conflict(LogEntity entry1, LogEntity entry2)
        {
            // Check if the day are equal.
            if (entry1.Day.CompareTo(entry2.Day) == 0)
            {
                // Check if the time conflicts.
                if ((
                    // Check if the from time is inside.
                    entry1.TimeFrom.CompareTo(entry2.TimeFrom) == -1 &&
                    entry1.TimeTo.CompareTo(entry2.TimeFrom) == 1) || (
                    // Check if the to time is inside.
                    entry1.TimeFrom.CompareTo(entry2.TimeTo) == -1 &&
                    entry1.TimeTo.CompareTo(entry2.TimeTo) == 1) || (
                    // Check if the to time is the same.
                    entry1.TimeFrom.CompareTo(entry2.TimeFrom) == 0 &&
                    entry1.TimeTo.CompareTo(entry2.TimeTo) == 0) || (
                    // Check if the time is over lapping.
                    entry1.TimeFrom.CompareTo(entry2.TimeFrom) == 1 &&
                    entry1.TimeTo.CompareTo(entry2.TimeTo) == -1))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetDuration(IEnumerable<LogEntity> entries)
        {
            if(entries == null)
            {
                return "0";
            }

            var hours = 0d;

            foreach (var entry in entries)
            {
                // Get the duration and return it in a nice format.
                var time = entry.TimeFrom.Subtract(entry.TimeTo);
                hours += time.Hours + (time.Minutes / 60d);
            }

            return Math.Round(Math.Abs(hours), 2).ToString();
        }

        public static string GetDuration(TimeSpan from, TimeSpan to)
        {
            // Get the duration and return it in a nice format.
            return from.Subtract(to).ToString(TimeSpanHourFormat);
        }

        public static List<string> GetListOfAvalibleDays(IEnumerable<LogEntity> entries)
        {
            // Create a set of dates for only getting one of each day.
            var dates = new SortedSet<DateTime>();
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Add the entry days to the set.
            entries.ToList().ForEach(entry => dates.Add(entry.Day));

            // Convert all entries in the set to string.
            foreach (var date in dates)
            {
                dateStrings.Add(date.ToString("yyyy - MM - dd"));
            }

            return dateStrings;
        }

        public static IList<string> GetListOfAvalibleWeeks(IEnumerable<LogEntity> entries)
        {
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Convert all entries in the set to string.
            foreach (var e in entries)
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

        public static IList<string> GetListOfAvalibleYear(IEnumerable<LogEntity> entries)
        {
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Convert all entries in the set to string.
            foreach (var e in entries)
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

        public static IList<string> GetListOfAvalibleMonths(IEnumerable<LogEntity> entries)
        {
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Convert all entries in the set to string.
            foreach (var e in entries)
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
