using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using MEB.EasyTimeLog.Domain;
using MEB.EasyTimeLog.DataAccess;
using MEB.EasyTimeLog.Model.Exception;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MEB.EasyTimeLog.Model
{
    public class LogRepository : IRepository<LogEntity, Guid>
    {
        public const string DataStoreName = "Logs";

        private readonly IDataStore<JObject> _dataStore;
        private readonly IRepository<TaskEntity, Guid> _taskRepository;
        private readonly Dictionary<Guid, LogEntity> _entities;

        public LogRepository(IDataStore<JObject> dataStore, IRepository<TaskEntity, Guid> taskRepository)
        {
            _dataStore = dataStore;
            _taskRepository = taskRepository;
            _entities = new Dictionary<Guid, LogEntity>();

            LoadEntities();
        }

        public LogEntity Get(Guid key)
        {
            return _entities[key];
        }

        public IList<LogEntity> GetAll()
        {
            var entities = _entities.Values.ToList();

            entities.Sort((e1, e2) => string.CompareOrdinal(e1.ToString(), e2.ToString()));

            return entities;
        }

        public IList<LogEntity> GetAll(string sortType, string sortValue)
        {
            var logEntities = new List<LogEntity>();

            switch (sortType)
            {
                case "Task":
                    // Load all logs into the properties.
                    logEntities.AddRange(_entities.Values.Where(e => e.TaskRef.Name == sortValue));
                    break;
                case "Day":
                    DateTime selectedDay;
                    if (DateTime.TryParse(sortValue, out selectedDay))
                    {
                        // Load all logs into the properties.
                        logEntities.AddRange(_entities.Values.Where(e => e.Day == selectedDay));
                    }
                    break;
                case "Week":
                    // Load all logs into the properties.
                    logEntities.AddRange(_entities.Values.Where(e => $"{e.Day.Year} - Week {TimeUtil.Calendar.GetWeekOfYear(e.Day, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}" == sortValue));
                    break;
                case "Month":
                    // Load all logs into the properties.
                    logEntities.AddRange(_entities.Values.Where(e => e.Day.ToString(TimeUtil.DateMonthFormat) == sortValue));
                    break;
                case "Year":
                    // Load all logs into the properties.
                    logEntities.AddRange(_entities.Values.Where(e => e.Day.Year.ToString() == sortValue));
                    break;
            }

            logEntities.Sort((entity1, entity2) => string.CompareOrdinal(entity1.ToString(), entity2.ToString()));

            return logEntities;
        }

        public LogEntity Save(LogEntity entity)
        {
            // Check if there is any conflicts.
            if (_entities.Values.Any(existingEntiry => TimeUtil.Conflict(existingEntiry, entity)))
            {
                // Throw a new exception, with the first conflict.
                // First is acceptable because there can be no conflicts. 
                throw new TimeConflictException(
                    _entities.Values.First(
                        existingEntiry => TimeUtil.Conflict(existingEntiry, entity)));
            }

            if (!_entities.ContainsKey(entity.Id))
            {
                Guid newId;
                do
                {
                    newId = Guid.NewGuid();
                } while (_entities.ContainsKey(newId));

                entity = new LogEntity(newId)
                {
                    TimeFrom = entity.TimeFrom,
                    TimeTo = entity.TimeTo,
                    Task = entity.Task,
                    Day = entity.Day,
                    TaskRef = _taskRepository.Get(entity.Task)
                };

                _entities.Add(newId, entity);
            }
            else
            {
                _entities[entity.Id] = entity;
            }

            SaveEntities();

            return entity;
        }

        public void Delete(Guid key)
        {
            if (!_entities.ContainsKey(key))
            {
                return;
            }

            _entities.Remove(key);

            SaveEntities();
        }

        public void LoadEntities()
        {
            var json = _dataStore.Load(DataStoreName);

            if (json == null)
            {
                return;
            }

            foreach (var child in json[DataStoreName].Children<JObject>())
            {
                var childJson = child.ToString(Formatting.None);
                var entity = TranslateFromJson(childJson);

                _entities.Add(entity.Id, entity);
            }
        }

        public void SaveEntities()
        {
            var json = new JObject
            {
                [DataStoreName] = new JArray()
            };

            foreach (var jsonEntity in _entities.Values.Select(TranslateToJson))
            {
                json[DataStoreName].Value<JArray>().Add(JObject.Parse(jsonEntity));
            }

            _dataStore.Save(DataStoreName, json);
        }

        public LogEntity TranslateFromJson(string jsonText)
        {
            var json = JObject.Parse(jsonText);

            var id = Guid.Parse(json[nameof(LogEntity.Id)].Value<string>());
            var taskId = Guid.Parse(json[nameof(LogEntity.Task)].Value<string>());

            var timeFrom = TimeSpan.Parse(json[nameof(LogEntity.TimeFrom)].Value<string>(), CultureInfo.InvariantCulture);
            var timeTo = TimeSpan.Parse(json[nameof(LogEntity.TimeTo)].Value<string>(), CultureInfo.InvariantCulture);
            var day = DateTime.Parse(json[nameof(LogEntity.Day)].Value<string>(), CultureInfo.InvariantCulture);

            var log = new LogEntity(id)
            {
                TimeFrom = timeFrom,
                TimeTo = timeTo,
                Day = day,
                Task = taskId,
                TaskRef = _taskRepository.Get(taskId)
            };

            return log;
        }

        public string TranslateToJson(LogEntity element)
        {
            var json = new JObject
            {
                [nameof(LogEntity.Id)] = element.Id,
                [nameof(LogEntity.TimeFrom)] = element.TimeFrom,
                [nameof(LogEntity.TimeTo)] = element.TimeTo,
                [nameof(LogEntity.Day)] = element.Day,
                [nameof(LogEntity.Task)] = element.Task
            };

            return json.ToString(Formatting.None);
        }
    }
}