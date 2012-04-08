using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace Vaccine.Core.Domain.Reporting
{
    public class BankAccountReport
    {
        public BankAccountReport()
        {

        }

        public virtual Guid Id { get; set; }

        public virtual Guid AggregateRootId { get; set; }

        public virtual int Version { get; set; }

        public virtual string AccountName { get; set; }

        public virtual decimal Balance { get; set; }
    }

}
