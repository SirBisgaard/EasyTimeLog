using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MEB.EasyTimeLog.DataAccess;
using MEB.EasyTimeLog.Domain;

namespace MEB.EasyTimeLog.Model
{
    public class RepositoryFactory
    {
        private static readonly LogRepository LogRepository = new LogRepository(new JsonDataStore());
        private static readonly TaskRepository TaskRepository = new TaskRepository(new JsonDataStore());

        public static IRepository<TE, TK> GetRepository<TE, TK>()
        {
            if (typeof(TK) != typeof(Guid))
            {
                return null;
            }

            if (typeof(TE) == typeof(LogEntity))
            {
                return (IRepository<TE, TK>)LogRepository;
            }
            if (typeof(TE) == typeof(TaskEntity))
            {
                return (IRepository<TE, TK>)TaskRepository;
            }

            return null;
        }
    }
}