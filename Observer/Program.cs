using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    class Program
    {
        static void Main(string[] args)
        {
            var relay = new Relay();
            var subscbribers = new List<IObserver<Message>>();
            for (int i = 0; i < 5; i++)
            {
                var sub = new Subscriber(i, relay);
                subscbribers.Add(sub);
            }
            for (int i = 0; i < 5; i++)
            {
                relay.Notify(new Message("Issue #" + i));
                subscbribers[i].Unsubscribe();
            }
            Console.WriteLine("Reading key...");
            System.Console.ReadKey();
        }

    }

    class Relay : IObservable<Message>
    {
        private readonly List<IObserver<Message>> _subscribers;

        public Relay()
        {
            _subscribers = new List<IObserver<Message>>();
        }

        public IDisposable Subscribe(IObserver<Message> subscriber)
        {
            _subscribers.Add(subscriber);
            return new Unsubscriber(subscriber, _subscribers);
        }
        

        public void Notify(Message message)
        {
            foreach (var sub in _subscribers)
            {
                sub.OnNext(message);
            }
        }
    }

    internal class Unsubscriber : IDisposable
    {
        private readonly IObserver<Message> _subscriber;
        private readonly List<IObserver<Message>> _subscribers;

        public Unsubscriber(IObserver<Message> subscriber, List<IObserver<Message>> subscribers)
        {
            _subscriber = subscriber;
            _subscribers = subscribers;
        }

        public void Dispose()
        {
            if (_subscriber != null && _subscribers.Contains(_subscriber))
                _subscribers.Remove(_subscriber);
        }
    }

    class Subscriber : IObserver<Message>
    {
        private readonly int _id;
        private IDisposable _unsubscriber;

        public Subscriber(int ID, IObservable<Message> relay)
        {
            _id = ID;
            _unsubscriber = relay.Subscribe(this);

        }

        public void OnNext(Message msg)
        {
            Console.WriteLine(_id + "\t received:\t" + msg.Text);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }

    internal class Message
    {
        public string Text { get; }

        public Message(string text)
        {
            Text = text;
        }
    }
}
