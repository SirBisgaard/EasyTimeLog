using MEB.EasyTimeLog.DataAccess.DataStore;
using MEB.EasyTimeLog.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MEB.EasyTimeLog.Model
{
    public class DomainRepository
    {
        private IDataStore _dataStore;
        private TimeSheet _currentSheet;

        public DomainRepository(IDataStore dataStore)
        {
            _dataStore = dataStore;
            _currentSheet = new TimeSheet();
        }

        public DomainRepository()
        {
            _dataStore = new JsonDataStore();
            _currentSheet = new TimeSheet();
        }

        public TaskEntry CreateTaskEntry(string name)
        {
            // Check if exist.
            if(TaskExist(name))
            {
                // Return the existing one.
                return _currentSheet.Tasks.First(task => task.Name == name);
            }

            // Create task entry.
            var entry = new TaskEntry
            {
                Name = name
            };

            // Store task entry.
            _currentSheet.Tasks.Add(entry);

            // Return entry.
            return entry;
        }

        public void CreateTimeEntry(TaskEntry task, TimeSpan from, TimeSpan to, DateTime day)
        {
            
        }

        public bool TaskExist(string name)
        {
            // Return if there are any task with the same name.
            return _currentSheet.Tasks.Any(task => task.Name == name);
        }

        public List<TaskEntry> GetAllTasks()
        {
            // Return the list from the time sheet.
            return _currentSheet.Tasks;
        }
    }
}
