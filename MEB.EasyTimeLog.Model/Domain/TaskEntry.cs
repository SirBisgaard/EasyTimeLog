using System;

namespace MEB.EasyTimeLog.Model.Domain
{
    public class TaskEntry
    {
        public TaskEntry()
        {
            Name = string.Empty;
        }

        public string Name { get; set; }
    }
}
