using MEB.EasyTimeLog.Model.Domain;

namespace MEB.EasyTimeLog.Model.Exception
{
    public class TimeConflictException : System.Exception
    {
        public TimeEntry ConflictingEntry { get; set; }
        
        public TimeConflictException(TimeEntry entry) : base("The time entry you are trying to save, is conflicting with an existing one.")
        {
            ConflictingEntry = entry;
        }
    }
}
