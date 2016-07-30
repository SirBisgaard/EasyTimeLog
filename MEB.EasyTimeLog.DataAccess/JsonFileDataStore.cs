using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MEB.EasyTimeLog.DataAccess
{
    public class JsonDataStore : IDataStore<JObject>
    {
        private readonly string _baseDirectory;

        public JsonDataStore()
        {
            _baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            CreateScope();
        }

        public JsonDataStore(string scope)
        {
            _baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scope);

            CreateScope();
        }

        private void CreateScope()
        {
            if (!Directory.Exists(_baseDirectory))
            {
                Directory.CreateDirectory(_baseDirectory);
            }
        }

        public JObject Load(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var path = Path.Combine(_baseDirectory, name + ".json");

            if (!File.Exists(path))
            {
                return null;
            }

            using (var fileStream = File.Open(path, FileMode.Open))
            using (var reader = new StreamReader(fileStream))
            {
                var fileContent = reader.ReadToEnd();
                return JObject.Parse(fileContent);
            }
        }

        public void Save(string name, JObject element)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }


            var path = Path.Combine(_baseDirectory, name + ".json");

            File.WriteAllText(path, element.ToString(Formatting.Indented));
        }
    }
}
