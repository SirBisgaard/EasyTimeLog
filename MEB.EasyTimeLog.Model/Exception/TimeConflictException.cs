using MEB.EasyTimeLog.Domain;

namespace MEB.EasyTimeLog.Model.Exception
{
    public class TimeConflictException : System.Exception
    {
        public LogEntity ConflictingEntry { get; set; }
        
        public TimeConflictException(LogEntity entry) : base("The time entry you are trying to save, is conflicting with an existing one.")
        {
            ConflictingEntry = entry;
        }
    }
}
