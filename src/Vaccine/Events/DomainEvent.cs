﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Aggregate;

namespace Vaccine.Events
{
    [Serializable]
    public class DomainEvent : IDomainEvent
    {
        public DomainEvent()
        {
            //AggregateRootId = aggregateRootId;
            //EventVersion = version;
            EventState = EventState.Sequence;
            Key = new Key { KeyName = this.GetType().Name };
        }

        public Guid AggregateRootId { get; set; }

        //public int EventVersion { get; set; }

        public EventState EventState { get; set; }

        public Key Key { get; set; }
    }

    [Serializable]
    public class AggregateCreatedEvent : DomainEvent
    {
        public AggregateCreatedEvent()//:base(aggregateRootId)
        {
            this.IsActive = true;
            EventState = EventState.New;
        }

        public bool IsActive { get; set; }
    }
}
