using System;
using System.Threading;

namespace EventAggregator
{
    class Program
    {
        /// <summary>
        /// Sub/Pub Implementation with middle-man Message Queue
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var relay = new Relay();
            var pub = new Publisher(relay);
            var sub = new Subscriber(0, relay);
            var sub2 = new Subscriber(1, relay);

            pub.PublishMessage(new Message("A"));
            pub.PublishMessage(new Message("B"));
            Thread.Sleep(1000);
            pub.PublishMessage(new Message("C"));
            Console.ReadKey();

        }
    }
}