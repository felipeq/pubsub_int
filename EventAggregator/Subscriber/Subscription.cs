using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EventAggregator
{
    /// <summary>
    /// Token for Subscriber - Relay relation.
    /// </summary>
    internal class Subscription
    {
        public readonly Subscriber Subscriber;
        /// <summary>
        /// FIFO collection of tasks to perform on Messages delivered to Subscriber.
        /// </summary>
        private readonly Queue<Task<bool>> _queue;

        /// <summary>
        /// As we dequeue from _queue to perform tasks asynchronously, 
        /// we need separate counter of _queue status, as _queue.Count is not relevant
        /// </summary>
        private int _queueCount;

        /// <summary>
        /// Thread-safe guard
        /// </summary>
        private readonly object _locker = new object();

        public Subscription(Subscriber subscriber)
        {
            Subscriber = subscriber;
            _queue = new Queue<Task<bool>>();
        }
        /// <summary>
        /// Notify subscriber with Message 
        /// </summary>
        /// <param name="message"></param>
        public void Notify(Message message)
        {
            lock (_locker)
            {
                _queue.Enqueue(Subscriber.ReceiveMsg(message));
                _queueCount++;
            }
            Console.WriteLine($"SUB {Subscriber.ID} notified about {message.Text}. In his queue: {_queueCount}");

            ProcessQueueAsync(message);
        }

        /// <summary>
        /// Process the queue.
        /// </summary>
        /// <param name="message"></param>

        public async void ProcessQueueAsync(Message message)
        {
            while (_queue.Count > 0)
            {
                Task<bool> currentTask;
                lock (_locker)
                {
                    currentTask = _queue.Dequeue();
                    Console.WriteLine($"SUB {Subscriber.ID} is about to process {message.Text}");
                    currentTask.Start();
                }
                await currentTask;

                lock (_locker)
                {
                    _queueCount--;
                }
            }
        }

    }
}