using System;
using System.IO;
using MEB.EasyTimeLog.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MEB.EasyTimeLog.Test.DataAccess
{
    [TestClass]
    public class DataStoreTest
    {
        private readonly string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test");

        [TestMethod]
        [TestCategory("JsonDataStore")]
        public void LoadShouldReturnNullIfFileNotExist()
        {
            var dataStore = new JsonDataStore();

            var jObject = dataStore.Load("File");

            Assert.IsNull(jObject);
        }

        [TestMethod]
        [TestCategory("JsonDataStore")]
        public void LoadShouldReturnObjectIfFileExist()
        {
            var path = Path.Combine(baseDirectory, "TestFile.json");

            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }

            using (var stream = File.Create(path))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("{}");
                writer.Flush();
                writer.Close();
            }

            var dataStore = new JsonDataStore("Test");

            var jObject = dataStore.Load("TestFile");

            Assert.IsNotNull(jObject);

            Assert.AreEqual("{}", jObject.ToString(Formatting.None));

            File.Delete(path);
        }

        [TestMethod]
        [TestCategory("JsonDataStore")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadShouldThorwExceptionIfNameIsNull()
        {
            var dataStore = new JsonDataStore("Test");
            dataStore.Load(null);
        }

        [TestMethod]
        [TestCategory("JsonDataStore")]
        public void SaveShouldSaveJsonFileWithEmptyContent()
        {
            var path = Path.Combine(baseDirectory, "Data.json");

            Assert.AreEqual(File.Exists(path), false);

            var dataStore = new JsonDataStore("Test");

            dataStore.Save("Data", new JObject());

            Assert.AreEqual(File.Exists(path), true);

            var content = File.ReadAllText(path);

            Assert.AreEqual(new JObject().ToString(Formatting.None), content);

            File.Delete(path);
        }


        [TestMethod]
        [TestCategory("JsonDataStore")]
        public void DataStoreShoulCreateDefaultFolder()
        {
            var defautlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (Directory.Exists(defautlPath))
            {
                Directory.Delete(defautlPath);
            }

            Assert.AreEqual(Directory.Exists(defautlPath), false);

            var dataStore = new JsonDataStore();

            Assert.AreEqual(Directory.Exists(defautlPath), true);

            Directory.Delete(defautlPath);
        }

        [TestMethod]
        [TestCategory("JsonDataStore")]
        public void DataStoreShoulCreateFolderFromScope()
        {
            var defautlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OtherTest");
            if (Directory.Exists(defautlPath))
            {
                Directory.Delete(defautlPath);
            }

            Assert.AreEqual(Directory.Exists(defautlPath), false);

            var dataStore = new JsonDataStore("OtherTest");

            Assert.AreEqual(Directory.Exists(defautlPath), true);

            Directory.Delete(defautlPath);
        }

        [TestMethod]
        [TestCategory("JsonDataStore")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveShouldThorwExceptionIfNameIsNull()
        {
            var dataStore = new JsonDataStore("Test");
            dataStore.Save(null, new JObject());
        }

        [TestMethod]
        [TestCategory("JsonDataStore")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveShouldThorwExceptionIfElementIsNull()
        {
            var dataStore = new JsonDataStore("Test");
            dataStore.Save("Data", null);
        }
    }
}
