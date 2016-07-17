using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEB.EasyTimeLog.DataAccess
{
    public interface IDataStore<TE> where TE : class
    {
        TE Load(string name);

        void Save(string name, TE element);
    }
}
