using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Core.Cqrs.Events
{
    public class StoredEvent
    {
        public StoredEvent()
        {

        }
        public virtual Guid Id { get; set; }

        public virtual Guid AggregateRootId { get; set; }

        public virtual Guid EventId { get; set; }

        public virtual int Version { get; set; }

        public virtual long RowVersion { get; set; }

        public virtual string EventType { get; set; }

        public virtual byte[] PayLoadEvent { get; set; }

        //public virtual byte[] Command { get; set; }

        public virtual DateTime TimeStamp { get; set; }

        //public virtual string EventSr { get; set; }
    }
}
