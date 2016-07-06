using System;
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
        public TaskEntry Work { get; set; }
    }
}
