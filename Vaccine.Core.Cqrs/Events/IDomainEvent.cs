using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Core.Cqrs.Events
{
    public  interface IDomainEvent
    {
        Guid AggregateRootId { get; set; }
        //int EventVersion { get; set; }
        EventState EventState { get; set; }
    }
}
