using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Events
{
    public interface IEventStoreTransaction : IDisposable
    {
        void Commit();
    }
}
