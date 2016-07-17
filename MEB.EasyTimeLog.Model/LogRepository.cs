using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MEB.EasyTimeLog.DataAccess;
using MEB.EasyTimeLog.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MEB.EasyTimeLog.Model
{
    public class LogRepository : IRepository<LogEntity>
    {
        private readonly IDataStore<JObject> _dataStore;
        private ConcurrentDictionary<Guid, LogEntity> _entities;


        public LogRepository(IDataStore<JObject> dataStore)
        {
            _dataStore = dataStore;

            _entities = LoadEntities();
        }

        private ConcurrentDictionary<Guid, LogEntity> LoadEntities()
        {
            var entities = new ConcurrentDictionary<Guid, LogEntity>();

            var json = _dataStore.Load("Logs");

            if (json == null)
            {
                return entities;
            }

            foreach (var child in json["Logs"].Children<JObject>())
            {
                var childJson = child.ToString(Formatting.None);
                var entity = TranslateFromJson(childJson);

                entities.TryAdd(entity.Id, entity);
            }

            return entities;
        }

        public LogEntity TranslateFromJson(string jsonText)
        {
            var json = new JObject();

            var id = Guid.Parse(json[nameof(LogEntity.Id)].Value<string>());
            var taskId = Guid.Parse(json[nameof(LogEntity.Task)][nameof(TaskEntity.Id)].Value<string>());

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