using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Core.Cqrs.Commands
{
    [Serializable]
    public class DomainCommand : ICommand
    {

        public Guid CommandId
        {
            get;
            set;
        }

        //public Guid ObjectId
        //{
        //    get;
        //    set;
        //}
    }
}
