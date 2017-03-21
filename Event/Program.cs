using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Event
{
    class Program
    {
        static void Main(string[] args)
        {
            var relay = new Relay();
            var subscribers = new List<ISubscriber>();
            for (var i = 0; i < 5; i++)
            {
                var subscriber = new Suscriber(i);
                subscribers.Add(subscriber);
                relay.Subscribe(subscriber);
            }
            for (int i = 0; i < 2; i++)
            {
                relay.Notify(Publisher.Time());
            }
            Console.WriteLine("Reading key...");
            System.Console.ReadKey();
        }
    }

    static class Publisher
    {
        public static string Time()
        {
            var time = DateTime.Now.ToLongTimeString();
            Console.WriteLine("[PUB] Publishing " + time);
            return time;
        }
    }
    class Relay : IRelay
    {
        public void Notify(Message message)
        {

            OnNotifier(message);
        }

        public void Subscribe(ISubscriber subscriber)
        {
            Notifier += subscriber.ReceiveMsg;
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeAll()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<Message> Notifier;

        void OnNotifier(Message message)
        {
            if (Notifier != null)
                Notifier(this, message);
        }

        public void Notify(string message)
        {
            Notify(new Message(message));
        }
    }

    class Suscriber : ISubscriber, IRemoteService
    {
        private const int Delay = 500;
        private readonly int _id;

        public Suscriber(int ID)
        {
            _id = ID;

        }

        Task<bool> ISubscriber.ReceiveMsg(Message message)
        {
            throw new NotImplementedException();
        }

        Task<HttpStatusCode> IRemoteService.ReceiveMsg(Message message)
        {
            throw new NotImplementedException();
        }

        public void ReceiveMsg(object sender, Message e)
        {
            Thread.Sleep(Delay);
            string time = DateTime.Now.ToLongTimeString();
            Console.WriteLine($"[SUB {_id}][{time}] got {e.Text}");

        }
    }

    internal interface ISubscriber
    {
        Task<bool> ReceiveMsg(Message message);
        void ReceiveMsg(object sender, Message e);
    }

    internal class Message
    {
        public string Text { get; }

        public Message(string message)
        {
            Text = message;
        }
    }

    internal interface IRemoteService
    {
        Task<HttpStatusCode> ReceiveMsg(Message message);
    }

    internal interface IRelay
    {
        event EventHandler<Message> Notifier;
        void Notify(Message message);
        void Subscribe(ISubscriber subscriber);
        void Unsubscribe(ISubscriber subscriber);
        void UnsubscribeAll();
    }
}
