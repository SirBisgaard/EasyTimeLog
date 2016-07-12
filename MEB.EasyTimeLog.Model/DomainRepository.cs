using MEB.EasyTimeLog.DataAccess.DataStore;
using MEB.EasyTimeLog.Model.Domain;
using MEB.EasyTimeLog.Model.Exception;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MEB.EasyTimeLog.Model
{
    public class DomainRepository
    {
        private IDataStore _dataStore;
        private TimeSheet _currentSheet = new TimeSheet();

        public DomainRepository(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public DomainRepository()
        {
            _dataStore = new JsonDataStore();
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
            // Create a new time instance.
            var newEntry = new TimeEntry
            {
                Task = task,
                TimeFrom = from,
                TimeTo = to,
                Day = day
            };

            // Check if there is any conflicts.
            if(_currentSheet.Logs.Any(entry => TimeUtil.Conflict(entry, newEntry)))
            {
                // Throw a new exception, with the first conflict.
                // First is acceptable because there can be no conflicts. 
                throw new TimeConflictException(
                    _currentSheet.Logs.First(
                        entry => TimeUtil.Conflict(entry, newEntry)));
            }

            _currentSheet.Logs.Add(newEntry);
        }

        public bool TaskExist(string name)
        {
            // Return if there are any task with the same name.
            return _currentSheet.Tasks.Any(task => task.Name == name);
        }

        public List<TimeEntry> GetAllLogs()
        {
            // Return the list from the time sheet.
            return _currentSheet.Logs;
        }

        public List<TaskEntry> GetAllTasks()
        {
            // Return the list from the time sheet.
            return _currentSheet.Tasks;
        }
    }
}
