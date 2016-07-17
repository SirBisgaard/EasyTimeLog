using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEB.EasyTimeLog.DataAccess
{
    public interface IDataStore<E> where E : class
    {
        E Load(string name);

        void Save(string name, E element);
    }
}
