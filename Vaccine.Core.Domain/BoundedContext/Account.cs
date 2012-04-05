using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaccine.Core.Cqrs.Events;
using Vaccine.Core.Cqrs.Aggregate;

namespace Vaccine.Core.Domain.BoundedContext
{
    [Serializable]
    public class BalanceDecreasedEvent : DomainEvent
    {
        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }

    [Serializable]
    public class BalanceIncreasedEvent : DomainEvent
    {
        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }

    [Serializable]
    public class AccountNameAndBalanceChangedEvent : DomainEvent
    {
        public decimal Balance { get; set; }
        public string AccountName { get; set; }
    }


    [Serializable]
    public class AccountCreatedEvent : DomainEvent
    {
        public AccountCreatedEvent(Guid aggregateRootId, int version)
        {
            this.AggregateRootId = aggregateRootId;

            this.EventVersion = version;
        }
        public decimal OpeningBalance { get; set; }

        public bool IsActive { get; set; }

        public string AccountName { get; set; }
    }

    [Serializable]
    public class Account : AggregateRootEs<Account>
    {
        public Account()
        {

        }

        public Account(IEnumerable<DomainEvent> events)
            : this()
        {
            this.ReplayEvent(events);

        }

        public Account(Guid Id, decimal balance, string accountName)
            : this()
        {
            var version = Version;
            this.ApplyEvent<AccountCreatedEvent>(new AccountCreatedEvent(Id, version) { IsActive = IsActive, OpeningBalance = balance, AccountName = accountName, EventState = EventState.New });
        }

        public decimal _balance;

        public string _accountName;

        public override void CreateSnapshot()
        {
            var version = this.Version + 1;
            this.ApplyEvent<SnapshotCreatedEvent>(new SnapshotCreatedEvent { AggregateRootId = AggregateRootId, SnapshotObject = new Account { _balance = _balance, _accountName = _accountName, AggregateRootId = AggregateRootId, Version = version }, EventVersion = version });
        }

        public override void OnSnapshotCreated(SnapshotCreatedEvent @event)
        {
            var account = (Account)@event.SnapshotObject;
            this.AggregateRootId = account.AggregateRootId;
            this.Version = account.Version;
            this._balance = account._balance;
            this._accountName = account._accountName;

        }

        public void ChangeAccountNameAndBalance(string accountName, decimal balance)
        {
            this.ApplyEvent<AccountNameAndBalanceChangedEvent>(new AccountNameAndBalanceChangedEvent { AggregateRootId = AggregateRootId, AccountName = accountName, Balance = balance });
        }

        public void DecreaseBalance(decimal amount)
        {
            var balance = this._balance - amount;
            this.ApplyEvent<BalanceDecreasedEvent>(new BalanceDecreasedEvent { AggregateRootId = AggregateRootId, Amount = amount, Balance = balance });
        }

        public void IncreaseBalance(decimal amount)
        {
            var balance = this._balance + amount;
            this.ApplyEvent<BalanceIncreasedEvent>(new BalanceIncreasedEvent { AggregateRootId = AggregateRootId, Amount = amount, Balance = balance });
        }

        private void OnAccountCreated(AccountCreatedEvent @event)
        {
            //this.CreateAggregate(@event.AggregateRootId);
            this.AggregateRootId = @event.AggregateRootId;
            this.Version = @event.EventVersion;
            this.IsActive = @event.IsActive;
            this._accountName = @event.AccountName;
            this._balance = @event.OpeningBalance;

        }

        private void OnBalanceIncreased(BalanceIncreasedEvent @event)
        {

            this._balance = @event.Balance;
        }

        private void OnBalanceDecreased(BalanceDecreasedEvent @event)
        {
            this._balance = @event.Balance;
        }

        private void OnAccountNameAndBalanceChanged(AccountNameAndBalanceChangedEvent @event)
        {
            this._accountName = @event.AccountName;
            this._balance = @event.Balance;
        }


    }
}
