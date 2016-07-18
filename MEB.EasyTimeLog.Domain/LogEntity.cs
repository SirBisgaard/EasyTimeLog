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
    }
}
