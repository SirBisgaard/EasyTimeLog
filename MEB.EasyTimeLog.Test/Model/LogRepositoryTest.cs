using System;
using MEB.EasyTimeLog.DataAccess;
using MEB.EasyTimeLog.Domain;
using MEB.EasyTimeLog.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MEB.EasyTimeLog.Test.Model
{
    [TestClass]
    public class LogRepositoryTest
    {
        [TestMethod]
        [TestCategory("LogRepository")]
        public void TestMethod1()
        {
            IRepository<LogEntity, Guid> repository = new LogRepository(new JsonDataStore());

            var testEntity = new LogEntity(Guid.Empty)
            {
                Day = DateTime.Today,
                TimeFrom = TimeSpan.MinValue,
                TimeTo = TimeSpan.MaxValue,
                Task = Guid.Empty
            };

            Console.WriteLine(repository.TranslateToJson(testEntity));
        }
    }
}
