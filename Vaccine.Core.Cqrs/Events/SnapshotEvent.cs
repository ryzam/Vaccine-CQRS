﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Core.Cqrs.Events
{
    [Serializable]
    public class SnapshotCreatedEvent : DomainEvent
    {
        public SnapshotCreatedEvent()
        {
            EventState = EventState.Snapshot;
            
        }
        public dynamic SnapshotObject { get; set; }

        
    }
}
