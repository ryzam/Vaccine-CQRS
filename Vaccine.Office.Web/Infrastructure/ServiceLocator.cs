﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vaccine.Core.Cqrs.Queue;
using Vaccine.Core.Cqrs.Commands;

namespace Vaccine.Office.Web.Infrastructure
{
    public class ServiceLocator
    {
        public static RabbitMQPublisher Pub { get; set; }
        //public static RabbitMQSubscriber Sub {get;set;}
        public static CommandBus Bus { get; set; }
    }
}