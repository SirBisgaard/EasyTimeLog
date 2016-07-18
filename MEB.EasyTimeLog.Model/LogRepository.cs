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
        private readonly Dictionary<Guid, LogEntity> _entities;

        public LogRepository(IDataStore<JObject> dataStore)
        {
            _dataStore = dataStore;
            _entities = new Dictionary<Guid, LogEntity>();

            LoadEntities();
        }

        public LogEntity Get(Guid key)
        {
            return _entities[key];
        }

        public IEnumerable<LogEntity> GetAll()
        {
            return _entities.Values;
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

            Guid newId;
            do
            {
                newId = Guid.NewGuid();    
            }
            while (_entities.ContainsKey(newId));

            var newEntity = new LogEntity(newId)
            {
                TimeFrom = entity.TimeFrom,
                TimeTo = entity.TimeTo,
                Task =  entity.Task,
                Day = entity.Day
            };

            _entities.Add(newId, newEntity);

            SaveEntities();

            return newEntity;
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
                Task = taskId
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