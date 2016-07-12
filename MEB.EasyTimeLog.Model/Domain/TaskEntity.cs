using MEB.EasyTimeLog.DataAccess;
using System;

namespace MEB.EasyTimeLog.Model.Domain
{
    public class TaskEntity : Entity
    {
        public TaskEntity() : base(new Guid())
        {
            Name = string.Empty;
        }

        public string Name { get; set; }
    }
}
