using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Events;

namespace Vaccine.Core.Cqrs.Commands
{
    /// <summary>
    /// Interface to discovers command handlers via reflection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandHandler<T> where T : ICommand
    {
        void Handle(T command);
    }

    /// <summary>
    /// Interface to discovers report handlers via reflection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReportViewHandler<T> where T : IDomainEvent
    {
        void Handle(T report);
    }
}
