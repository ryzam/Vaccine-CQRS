using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Events;

namespace Vaccine.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        void Handle(T command);
    }

    public interface IReportViewHandler<T> where T : IDomainEvent
    {
        void Handle(T report);
    }
}
