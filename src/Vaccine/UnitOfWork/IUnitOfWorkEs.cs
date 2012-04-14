﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Aggregate;

namespace Vaccine.UnitOfWork
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
