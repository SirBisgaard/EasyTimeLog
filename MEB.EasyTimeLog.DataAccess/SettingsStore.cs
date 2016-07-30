using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MEB.EasyTimeLog.DataAccess
{
    public class SettingsStore : ISettingsStore<string, string>
    {
        private readonly IDataStore<JObject> _dataStore;
        private readonly Dictionary<string, string> _dictionary;

        public SettingsStore(IDataStore<JObject> dataStore)
        {
            _dataStore = dataStore;

            _dictionary = LoadData();
        }

        public string Get(string key)
        {
            string value;
            if (!_dictionary.TryGetValue(key, out value))
                value = string.Empty;

            return value;
        }

        public void Save(string key, string value)
        {
            if (_dictionary.ContainsKey(key))
                _dictionary[key] = value;
            else
                _dictionary.Add(key, value);

            SaveData();
        }

        private void SaveData()
        {
            var data = new JObject();

            // Add each pair from the dictionary to the json object.
            foreach (var pair in _dictionary)
            {
                // Set the data.
                data[pair.Key] = pair.Value;
            }

            // Save the json object into the settings file.
            _dataStore.Save("Settings", data);
        }

        private Dictionary<string, string> LoadData()
        {
            // Load the data from the local storage.
            var data = _dataStore.Load("Settings");

            // Instantiate the new dictionary.
            var dictionary = new Dictionary<string, string>();

            // Get all the property names.
            if (data == null)
            {
                return dictionary;
            }

            // Get all the json property names from the data.
            var keys = data.Properties().Select(p => p.Name).ToList();
            // Add each property to the dictionary.
            foreach (var key in keys)
            {
                // Get the value.
                var value = data[key].Value<string>();

                // Add the property key and value to the dictionary.
                dictionary.Add(key, value);
            }

            // Return the dictionary with the settings.
            return dictionary;
        }
    }
}