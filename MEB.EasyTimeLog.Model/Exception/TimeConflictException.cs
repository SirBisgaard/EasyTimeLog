using MEB.EasyTimeLog.Model.Domain;

namespace MEB.EasyTimeLog.Model.Exception
{
    public class TimeConflictException : System.Exception
    {
        public TimeEntity ConflictingEntry { get; set; }
        
        public TimeConflictException(TimeEntity entry) : base("The time entry you are trying to save, is conflicting with an existing one.")
        {
            ConflictingEntry = entry;
        }
    }
}
