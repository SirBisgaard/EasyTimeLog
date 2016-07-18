using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using MEB.EasyTimeLog.Domain;
using MEB.EasyTimeLog.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MEB.EasyTimeLog.Model
{
    public class TaskRepository : IRepository<TaskEntity, Guid>
    {
        public const string DataStoreName = "Tasks";

        private readonly IDataStore<JObject> _dataStore;
        private readonly Dictionary<Guid, TaskEntity> _entities;

        public TaskRepository(IDataStore<JObject> dataStore)
        {
            _dataStore = dataStore;
            _entities = new Dictionary<Guid, TaskEntity>();

            LoadEntities();
        }

        public TaskEntity Get(Guid key)
        {
            return _entities[key];
        }

        public IEnumerable<TaskEntity> GetAll()
        {
            return _entities.Values;
        }

        public TaskEntity Save(TaskEntity entity)
        {
            var existingEntity = _entities.Values
                .FirstOrDefault(t => string.Equals(t.Name, entity.Name));

            if (existingEntity != null)
            {
                return existingEntity;
            }

            Guid newId;
            do
            {
                newId = Guid.NewGuid();
            }
            while (_entities.ContainsKey(newId));

            var newEntity = new TaskEntity(newId)
            {
                Name = entity.Name
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

        public TaskEntity TranslateFromJson(string jsonText)
        {
            var json = JObject.Parse(jsonText);

            var id = Guid.Parse(json[nameof(TaskEntity.Id)].Value<string>());

            var name = json[nameof(TaskEntity.Name)].Value<string>();

            var task = new TaskEntity(id)
            {
                Name = name
            };

            return task;
        }

        public string TranslateToJson(TaskEntity element)
        {
            var json = new JObject
            {
                [nameof(TaskEntity.Id)] = element.Id,
                [nameof(TaskEntity.Name)] = element.Name
            };

            return json.ToString(Formatting.None);
        }
    }
}