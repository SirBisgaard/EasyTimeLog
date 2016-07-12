using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEB.EasyTimeLog.Model
{
    public class DomainFactory
    {
        private static DomainRepository _domainRepo;

        // Basically a singleton moved into a factory..
        public static DomainRepository GetRepository()
        {
            // The if the repository null, just instantiate a new one.
            if(_domainRepo == null)
            {
                _domainRepo = new DomainRepository();
            }

            // Return the repository instance.
            return _domainRepo;
        }
    }
}
