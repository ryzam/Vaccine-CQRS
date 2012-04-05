using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Events;
using Vaccine.Core.Cqrs.Queue;
using MediuCms.Core.Cqrs.Commands;

namespace Vaccine.Core.Cqrs.Commands
{
    public class CommandBus
    {
        private IDictionary<Type, Action<dynamic>> handlers = new Dictionary<Type, Action<dynamic>>();

        private IList<ICommand> commands = new List<ICommand>();

        private int sendTry = 0;

        private IQuePublisher pub;

        public CommandBus()
        {

        }

        public CommandBus(IQuePublisher pub)
        {
            this.pub = pub;
        }

        public void RegisterHandlerCommand<TCommand>(Action<TCommand> cmd) where TCommand : ICommand
        {
            this.handlers.Add(typeof(TCommand), c => cmd((TCommand)c));
        }

        public void RegisterHandlerEvent<TEvent>(Action<TEvent> cmd) where TEvent : IDomainEvent
        {
            this.handlers.Add(typeof(TEvent), c => cmd((TEvent)c));
        }

        public void Send(ICommand cmd)
        {
            //if (sendTry <= 3)
            //{
            //Give command unique Id
            cmd.CommandId = SequentialGuid.NewGuid();

            commands.Add(cmd);

            foreach (var e in this.handlers.Where(c => c.Key == cmd.GetType()))
            {
                e.Value(cmd);
            }

            sendTry++;

            commands.Clear();
            //}
            //else
            //{
            //    throw new Exception("Something happen. Please try again");
            //}
        }

        public void Publish(IDomainEvent @event)
        {
            if (this.pub == null)
            {
                foreach (var e in this.handlers.Where(c => c.Key == @event.GetType()))
                {
                    e.Value(@event);
                }
            }
            else
            {

                this.pub.Send(@event);

            }
        }


        public ICommand GetCommandById(Guid Id)
        {
            return commands.Where(c => c.CommandId == Id).FirstOrDefault();
        }

    }
}