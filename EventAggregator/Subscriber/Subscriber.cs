using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventAggregator
{
    // todo subscribing to different message Types
    internal class Subscriber : ISubscriber
    {
        /// <summary>
        /// Subscriber's ID. NOT unique.
        /// </summary>
        public readonly int ID;

        /// <summary>
        /// Register subscriber inside relay
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="relay"></param>
        public Subscriber(int ID, Relay relay)
        {
            this.ID = ID;
            relay.Subscribe(new Subscription(this));
        }
        /// <summary>
        /// Message Handler
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Task that performs action on message</returns>
        public Task<bool> ReceiveMsg(Message msg)
        {
            return new Task<bool>(() => IsDbTransactionSuccessful(msg));
        }

        /// <summary>
        /// For science reasons
        /// </summary>
        private bool IsDbTransactionSuccessful(Message msg)
        {
            Thread.Sleep(new Random().Next(5000));
            Console.WriteLine($"SUB {ID} processed {msg.Text}");
            return true;

        }
    }
}