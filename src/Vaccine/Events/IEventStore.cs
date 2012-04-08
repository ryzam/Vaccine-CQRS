using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Aggregate;
using Vaccine.Events;

namespace Vaccine.Events
{
    public interface IEventStore
    {
        IEventStoreTransaction Start();
        void Save<T>(AggregateRootEs<T> root, Guid commandId);
        void Save<T>(AggregateRootEs<T> root);
        T GetById<T>(Guid Id) where T : AggregateRootEs<T>, new();
        IEnumerable<DomainEvent> GetEventsByAggregateId(Guid Id);
    }
}
