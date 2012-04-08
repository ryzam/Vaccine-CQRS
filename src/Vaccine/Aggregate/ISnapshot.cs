using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Events;

namespace Vaccine.Aggregate
{
    public interface ISnapshot
    {
        void CreateSnapshot();
        void OnSnapshotCreated(SnapshotCreatedEvent @event);
    }
}
