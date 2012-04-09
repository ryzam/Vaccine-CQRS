using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Commands;

namespace Vaccine.Core.Domain.BoundedContext.CustomerRegistration
{
    public class CreateCustomerCommand : DomainCommand
    {
       
        public CreateCustomerCommand(string name,string email)
        {
            this.name = name;
            this.email = email;
        }

        public string name;

        public string email;

        
    }
}
