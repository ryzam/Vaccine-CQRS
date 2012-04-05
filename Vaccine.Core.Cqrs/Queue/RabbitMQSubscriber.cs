using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;
using Vaccine.Core.Cqrs.Infrastructure;
using Vaccine.Core.Cqrs.Commands;

namespace Vaccine.Core.Cqrs.Queue
{
    public class RabbitMQSubscriber
    {
        ConnectionFactory factory;
        IConnection conn;
        CommandBus bus;

        public RabbitMQSubscriber()
        {

        }

        public RabbitMQSubscriber(CommandBus bus)
        {
            this.bus = bus;
            Console.WriteLine("Creating factory...");
            factory = new ConnectionFactory();

            Console.WriteLine("Creating connection...");
            factory.Protocol = Protocols.FromEnvironment();
            factory.HostName = "localhost";
            conn = factory.CreateConnection();

            
        }

        public void Listen()
        {
            Console.WriteLine("Creating channel...");
            using (IModel model = conn.CreateModel())
            {
                var subscription = new Subscription(model, "queue", false);
                //while (true)
                //{
                    BasicDeliverEventArgs basicDeliveryEventArgs =
                        subscription.Next();
                    var command = StreamExtension.Deserialize<Vaccine.Core.Cqrs.Commands.DomainCommand>(basicDeliveryEventArgs.Body);

                    //Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);
                    //Console.WriteLine(((CreateCommand)messageContent).Name);
                    bus.Send(command);

                    subscription.Ack(basicDeliveryEventArgs);
                //}
            }
        }

        public static void DbConfig()
        {
            throw new NotImplementedException();
        }
    }
}
