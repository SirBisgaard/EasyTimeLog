﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEB.EasyTimeLog.Model.Domain
{
    public class TimeEntry
    {
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo{ get; set; }
        public DateTime Day { get; set; }
        public TaskEntry Task { get; set; }

        public string ToString(string format)
        {
            return string.Format(
                format, 
                Task.Name, 
                Day, 
                TimeFrom.ToString(TimeUtil.TimeSpanFormat), 
                TimeTo.ToString(TimeUtil.TimeSpanFormat), 
                TimeUtil.GetDuration(TimeFrom, TimeTo));
        }
    }
}
