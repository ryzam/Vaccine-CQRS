using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Aggregate;

namespace Vaccine.Core.Cqrs.UnitOfWork
{
    public interface IUnitOfWorkEs
    {
        IUnitOfWorkEs StartUnitOfWork();

        void Save<T>(AggregateRootEs<T> root, Guid commandId);

        T GetById<T>(Guid Id);

        IQueryable<T> Gets<T>();

        void Commit();
    }
}
