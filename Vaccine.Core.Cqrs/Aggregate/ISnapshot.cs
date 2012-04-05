using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Events;

namespace Vaccine.Core.Cqrs.Aggregate
{
    public interface ISnapshot
    {
        void CreateSnapshot();
        void OnSnapshotCreated(SnapshotCreatedEvent @event);
    }
}
