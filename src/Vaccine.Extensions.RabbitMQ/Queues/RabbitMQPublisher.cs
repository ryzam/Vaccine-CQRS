using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using Vaccine.Commands;
using Vaccine.Infrastructure;
using RabbitMQ.Client.Framing.v0_8;
using Vaccine.Events;

namespace Vaccine.Queue
{
    public class RabbitMQPublisher : IQueuePublisher
    {
        ConnectionFactory factory;
        IConnection conn;
        IModel model;

        public RabbitMQPublisher(string connection)
        {
            factory = new ConnectionFactory();

            if (connection != null && connection != string.Empty)
            {
                factory.Uri = connection;
            }
            else
            {
                factory.HostName = "localhost";
            }

            Console.WriteLine("Creating connection...");
            factory.Protocol = Protocols.FromEnvironment();
           
            conn = factory.CreateConnection();

            Console.WriteLine("Creating channel...");
            model = conn.CreateModel();

            Console.WriteLine("Creating exchange...");
            model.ExchangeDeclare("exch", ExchangeType.Direct);

            Console.WriteLine("Creating queue...");
            model.QueueDeclare("queue", true, false, true, null);

            model.QueueBind("queue", "exch", "key", new Dictionary<string, object>());

        }

        public void Send(ICommand c)
        {
           
            byte[] messageBody = StreamExtension.Serialize(c);

            IBasicProperties basicProperties = model.CreateBasicProperties();

            model.BasicPublish("exch", "key", basicProperties, messageBody);

        }

        public void Send(IDomainEvent e)
        {

            byte[] messageBody = StreamExtension.Serialize(e);

            IBasicProperties basicProperties = model.CreateBasicProperties();

            model.BasicPublish("exch", "key", basicProperties, messageBody);

        }

        public void Close()
        {
            model.Close(Constants.ReplySuccess, "Closing the channel");

            conn.Close(Constants.ReplySuccess, "Closing the connection");
        }
    }
}
