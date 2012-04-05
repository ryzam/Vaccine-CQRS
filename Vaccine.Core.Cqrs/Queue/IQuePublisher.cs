using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Events;
using Vaccine.Core.Cqrs.Commands;

namespace Vaccine.Core.Cqrs.Queue
{
    public interface IQuePublisher
    {
        void Send(IDomainEvent e);

        void Send(ICommand c);
    }
}
