using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Events;
using Vaccine.Commands;

namespace Vaccine.Queue
{
    public interface IQuePublisher
    {
        void Send(IDomainEvent e);

        void Send(ICommand c);
    }
}
