using System;

namespace MEB.EasyTimeLog.Domain
{
    public class TaskEntity : Entity
    {
        public TaskEntity(Guid id) : base(id)
        {
            Name = string.Empty;
        }

        public TaskEntity() : base(Guid.Empty)
        {
            Name = string.Empty;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}