using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEB.EasyTimeLog.DataAccess
{
    public abstract class Entity
    {
        private Guid _id;

        internal Guid ID
        {
            get { return _id; }
        }

        public Entity(Guid id)
        {
            _id = id;
        }
    }
}
