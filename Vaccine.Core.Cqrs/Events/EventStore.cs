using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Vaccine.Core.Cqrs.Aggregate;
using Vaccine.Core.Cqrs.Commands;
using NHibernate;
using NHibernate.Linq;
using Vaccine.Core.Cqrs.Infrastructure;

namespace Vaccine.Core.Cqrs.Events
{
    public class EventStore
    {
        CommandBus _bus;

        public ISessionFactory _sf;

        public ISession _s;

        public EventStore(CommandBus bus,ISessionFactory sf)
        {
            _bus = bus;
            _sf = sf;
        }

        public ISession OpenSession()
        {
            _s = _sf.OpenSession();
            return _s;
        }

        public void Save<T>(AggregateRootEs<T> root, Guid commandId)
        {
            var events = root.GetUncommitedEvents();

            var uncomittedVersion = events.Count();

            var comittedVersion = root.GetCommitedEvents().Count();

            var latestEventVersion = root.Version + comittedVersion;

            var eventVersion = comittedVersion;

            var aggregateVersion = _s.Get<AggregateVersion>(root.AggregateRootId);

            if (aggregateVersion != null)
            {
                var expectedVersion = aggregateVersion.LastestEventVersion + uncomittedVersion;

                if (expectedVersion != latestEventVersion)
                {
                    Console.WriteLine("Concurrency Problem");

                    //var latestEvents = GetEventsByAggregateId(root.AggregateRootId).Where(c => c.EventVersion > comittedVersion);

                    //Retry Command
                    if (commandId != Guid.Empty)
                    {
                        root.ClearEvents();

                        var cmd = _bus.GetCommandById(commandId);

                        _bus.Send(cmd);
                    }

                }

                aggregateVersion.LastestEventVersion++;

                _s.Flush();

                
            }

            if (root.AggregateRootId == Guid.Empty)
            {
                root.Version = 1;

                root.AggregateRootId = Guid.NewGuid();

                var _aggregateVersion = new AggregateVersion { Id = root.AggregateRootId, Version = root.Version, TypeName = root.GetTypeName(), LastestEventVersion = latestEventVersion };

                _s.Save(_aggregateVersion);
            }
            
            foreach (var e in events)
            {
                eventVersion++;

                e.AggregateRootId = root.AggregateRootId;

                //e.EventVersion = eventVersion;

                //if (e.EventState == EventState.New)
                //{
                //    root.Version = 1;

                //    var _aggregateVersion = new AggregateVersion { Id = root.AggregateRootId, Version = root.Version, TypeName = root.GetTypeName(), LastestEventVersion = latestEventVersion };

                //    _s.Save(_aggregateVersion);

                //}
               
                if (e.EventState == EventState.Snapshot)
                {
                    aggregateVersion.Version++;
                    root.Version = aggregateVersion.Version;
                }
                
                var storedEvent = new StoredEvent
                    {
                        Id = SequentialGuid.NewGuid(),
                        AggregateRootId = root.AggregateRootId,
                        EventId = SequentialGuid.NewGuid(),
                        EventType = e.GetType().Name,
                        RowVersion = DateTimeOffset.UtcNow.UtcTicks,
                        TimeStamp = DateTime.Now,
                        Version = root.Version,
                        PayLoadEvent = StreamExtension.Serialize(e)
                    };
                
                _s.Save(storedEvent);

                _s.Flush();

                if (_bus != null)
                    _bus.Publish(e);
                    
            }
 
            root.ClearEvents();
        }


        public void Save<T>(AggregateRootEs<T> root)
        {
            Save<T>(root, Guid.Empty);
        }

        //private static int ReAssignVersion<T>(AggregateRootEs<T> root, DomainEvent e, int version, AggregateVersion aggregate)
        //{
        //    version = aggregate.Version;

        //    e.EventVersion = version;

        //    root.Version = version;

        //    e.AggregateRootId = root.AggregateRootId;

        //    return version;
        //}

        public T GetById<T>(Guid Id) where T : AggregateRootEs<T>, new()
        {
           
                var aggregate = _s.Get<AggregateVersion>(Id);

                var @events = _s.Query<StoredEvent>()
                                .Where(c => c.AggregateRootId == aggregate.Id && c.Version == aggregate.Version)
                                .AsEnumerable()
                                .Select(x => StreamExtension.Deserialize<DomainEvent>(x.PayLoadEvent));
                                //.Select(x => ServiceStack.Text.TypeSerializer.DeserializeFromStream<DomainEvent>(GetStream(x.PayLoadEvent)));
                                

                T t = new T();
                t.AggregateRootId = aggregate.Id;
                t.Version = aggregate.Version;

                t.ReplayEvent(@events);

                _s.Evict(aggregate);

                return t;
            
        }

        public IEnumerable<DomainEvent> GetEventsByAggregateId(Guid Id)
        {

            var aggregate = _s.Get<AggregateVersion>(Id);

            var @events = _s.Query<StoredEvent>()
                            .Where(c => c.AggregateRootId == aggregate.Id && c.Version == aggregate.Version)
                            .AsEnumerable()
                            .Select(x => StreamExtension.Deserialize<DomainEvent>(x.PayLoadEvent));

            return @events;
        }


        

    }
}
