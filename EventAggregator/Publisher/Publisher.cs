namespace EventAggregator
{
    // todo extract interface
    // todo Publisher factory on message Types
    internal class Publisher
    {
        readonly Relay _relay;
        /// <summary>
        /// Register publisher inside relay
        /// </summary>
        /// <param name="relay"></param>
        public Publisher(Relay relay)
        {
            _relay = relay;
        }
        // Publish Message to relay
        public void PublishMessage(Message msg)
        {
            _relay.Publish(msg);
        }
    }
}