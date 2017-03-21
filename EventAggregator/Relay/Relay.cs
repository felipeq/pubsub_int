using System;
using System.Collections.Generic;

namespace EventAggregator
{
    //todo singleton
    //todo support for multiple message Types
    //todo verbose
    //todo exception handling
    /// <summary>
    /// Message manager in Publisher to Subscriber relation.
    /// Subscriber registers in relay using Subscription token
    /// Publisher -> Relay -> Multiple Subscribers
    /// </summary>
    internal class Relay
    {
        private readonly List<Subscription> _subscriptions;

        public Relay()
        {
            _subscriptions = new List<Subscription>();
        }

        /// <summary>
        /// Publish Message to every subscribed subscriber
        /// </summary>
        /// <param name="message">Msg to publish</param>
        public void Publish(Message message)
        {
            Console.WriteLine($"Publishing {message.Text}. Total subscriptions for that message: {_subscriptions.Count}");
            foreach (var sub in _subscriptions)
            {
                sub.Notify(message);
            }
            Console.WriteLine($"Publishing of {message.Text} ended.");


        }
        /// <summary>
        /// Subscribe for Message receiving
        /// </summary>
        /// <param name="subscription">Subscription Token</param>
        /// 
        internal void Subscribe(Subscription subscription)
        {
            if (!_subscriptions.Contains(subscription))
                _subscriptions.Add(subscription);
            else
                throw new ArgumentException("Can't subscribe! Duplicates are not supported!");
        }
        /// <summary>
        /// Remove subscription
        /// </summary>
        /// <param name="subscription"></param>
        internal void Unsubscribe(Subscription subscription)
        {
            if (_subscriptions.Contains(subscription))
                _subscriptions.Remove(subscription);
            else
            {
                Console.WriteLine("Can't unsubscribe! Subscription not found!");
            }
        }
    }
}