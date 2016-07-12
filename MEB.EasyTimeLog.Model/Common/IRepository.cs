using MEB.EasyTimeLog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEB.EasyTimeLog.Model
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity Get(TKey id);
        void Save(TEntity entity);
        void Delete(TKey id);
        void Delete(TEntity entity);
    }
}
