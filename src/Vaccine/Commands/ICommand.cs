using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaccine.Commands
{
    public interface ICommand
    {
        
        Guid CommandId { get; set; }
    }
}
