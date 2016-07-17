using MEB.EasyTimeLog.DataAccess;
using System;

namespace MEB.EasyTimeLog.Model.Domain
{
    public class TaskEntity
    {
        public TaskEntity() 
        {
            Name = string.Empty;
        }

        public string Name { get; set; }
    }
}
