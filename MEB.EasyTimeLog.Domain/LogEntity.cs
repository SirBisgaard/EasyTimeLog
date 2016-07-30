using System;

namespace MEB.EasyTimeLog.Domain
{
    public class LogEntity : Entity
    {
        public LogEntity(Guid id) : base(id)
        {

        }

        public LogEntity() : base(Guid.Empty)
        {

        }

        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public DateTime Day { get; set; }
        public Guid Task { get; set; }
        public string Notes { get; set; }
        public TaskEntity TaskRef { get; set; }

        public override string ToString()
        {
            var _string = $"{Day.ToShortDateString()}:  {TimeFrom.ToString(@"hh\:mm")}-{TimeTo.ToString(@"hh\:mm")}  {TaskRef.Name}  {GetDuration()}";

            if(!string.IsNullOrEmpty(Notes))
            {
                _string += $"  ({Notes})";
            }

            return _string;
        }

        public double GetDuration()
        {
            var time = TimeTo.Subtract(TimeFrom);
            return time.Hours + Math.Round(time.Minutes / 60d, 2);
        }
    }
}
