using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dynamic.Role;
using Vaccine.Core.Cqrs.Events;
using System.Reflection;
using Vaccine.Core.Cqrs.Infrastructure;


namespace Vaccine.Core.Cqrs.Aggregate
{
    [Serializable]
    public class Key
    {
        public string KeyName { get; set; }
    }

    [Serializable]
    public abstract class AggregateRootEs<T> : ISnapshot, IRolePlayer
    {

        public Guid AggregateRootId { get; set; }

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public bool IsSnapshot { get; set; }


        [NonSerialized]
        public IDictionary<Type, Action<Key, DomainEvent>> handlers = new Dictionary<Type, Action<Key, DomainEvent>>();

        [NonSerialized]
        private IList<DomainEvent> events = new List<DomainEvent>();

        [NonSerialized]
        private IList<DomainEvent> historyEvents = new List<DomainEvent>();

        public AggregateRootEs()
        {
            IsSnapshot = false;
            IsActive = true;
            Version = 1; // Always new created object start with 1
            RegisterHandler();
        }

        public void RegisterHandler()
        {
            foreach (var m in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(c => c.ReturnType == typeof(void)).Select(x => new ReflectionHandler { Type = x.GetType(), Name = x.Name, Parameters = x.GetParameters().ToArray() }))
            {
                if (m.Name.StartsWith("On"))
                {

                    if (this.handlers.Where(c => c.Key == m.Parameters[0].ParameterType).Count() == 0)
                    {
                        var k = new Key { KeyName = m.Name };

                        Action<Key, DomainEvent> d = (key, parameter) => this.GetType().GetMethod(k.KeyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, new object[] { parameter }); //(Action<DomainEvent>)Delegate.CreateDelegate(typeof(Action<DomainEvent>), this, mi);

                        this.handlers.Add(m.Parameters[0].ParameterType, d);
                    }
                }
            }
        }

        public void ApplyEvent<TEvent>(TEvent @event) where TEvent : DomainEvent
        {
            ApplyEvent<TEvent>(@event, true);
        }

        public void ApplyEvent<TEvent>(TEvent @event, bool isNew) where TEvent : DomainEvent
        {
            

            if (@event.GetType() != typeof(SnapshotCreatedEvent) && IsSnapshot == false && isNew)
            {
                StartSnapshot(historyEvents);
            }

            foreach (var e in this.handlers.Where(c => c.Key == @event.GetType()))
            {
                e.Value(@event.Key, @event);
            }

            if (isNew)
                events.Add(@event);


        }

        public void ReplayEvent(IEnumerable<DomainEvent> events)
        {
            foreach (var e in events)
            {
                historyEvents.Add(e);
                ApplyEvent(e, false);
            }

        }

        private void StartSnapshot(IEnumerable<IDomainEvent> events)
        {
            if (events.Count() > 3)
            {
                IsSnapshot = true;
                CreateSnapshot();
            }
        }

        public IList<DomainEvent> GetCommitedEvents()
        {
            return historyEvents;
        }

        public IList<DomainEvent> GetUncommitedEvents()
        {
            return events;
        }

        public string GetTypeName()
        {
            return this.GetType().Name;
        }


        public void ClearEvents()
        {
            events.Clear();
        }

        public abstract void CreateSnapshot();

        public abstract void OnSnapshotCreated(SnapshotCreatedEvent @event);

    }


}
