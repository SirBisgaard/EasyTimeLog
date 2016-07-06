using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEB.EasyTimeLog.Model.Domain
{
    public class TimeSheet
    {
        public TimeSheet()
        {
            Logs = new List<TimeEntry>();
            Tasks = new List<TaskEntry>();
        }

        public List<TimeEntry> Logs { get; set; }
        public List<TaskEntry> Tasks { get; set; }
    }
}
