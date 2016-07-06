using MEB.EasyTimeLog.Model.Domain;
using System;
using System.Collections.Generic;

namespace MEB.EasyTimeLog.Model
{
    public class TimeUtil
    {
        public const string TimeSpanFormat = @"hh\:mm";
        public const string TimeSpanHourFormat = @"h\:mm";
        public const string DefaultTimeEntryFormat = "{0}: {2} - {3} Duration={4}";

        public static bool Conflict(TimeEntry entry1, TimeEntry entry2)
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
                    // Check if the time is over lapping.
                    entry1.TimeFrom.CompareTo(entry2.TimeFrom) == 1 &&
                    entry1.TimeTo.CompareTo(entry2.TimeTo) == -1))
                {
                    return true;
                }
            }

            return false;
        }

        internal static string GetDuration(TimeSpan from, TimeSpan to)
        {
            // Create simple date times from time spans.
            var fromDate = new DateTime(1, 1, 1, from.Hours, from.Minutes, 0);
            var toDate = new DateTime(1, 1, 1, to.Hours, to.Minutes, 0);

            // Get the duration and return it in a nice format.
            return from.Subtract(to).ToString(TimeSpanHourFormat);
        }

        public static List<string> GetListOfAvalibleDays(List<TimeEntry> entries)
        {
            // Create a set of dates for only getting one of each day.
            var dates = new SortedSet<DateTime>();
            // Create a list to contain all date strings.
            var dateStrings = new List<string>();

            // Add the entry days to the set.
            entries.ForEach(entry => dates.Add(entry.Day));

            // Convert all entries in the set to string.
            foreach(var date in dates)
            {
                dateStrings.Add(date.ToShortDateString());
            }         

            return dateStrings;
        }
    }
}
