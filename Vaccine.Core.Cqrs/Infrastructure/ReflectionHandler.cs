using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MediuCms.Core.Cqrs.Infrastructure
{
    public class ReflectionHandler
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public ParameterInfo[] Parameters { get; set; }
    }
}
