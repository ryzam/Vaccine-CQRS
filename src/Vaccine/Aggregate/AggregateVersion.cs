using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Aggregate
{
    public class AggregateVersion
    {
        public AggregateVersion()
        {
            this.UpdatedOn = DateTime.Now;
        }

        /// <summary>
        /// Will be used as AggregateID
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Object Type
        /// </summary>
        public virtual string TypeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int LastestEventVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime UpdatedOn { get; set; }
    }
}
