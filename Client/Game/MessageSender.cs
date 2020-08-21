namespace CardGame.Client.Game.Zones
{
    public class MessageSender
    {
        // Controls Messages Being Sent To The Server
        private const int ServerId = 1;
        private readonly Messenger Messenger;
        private int Id => Messenger.Multiplayer.GetNetworkUniqueId();

        public MessageSender(Messenger messenger)
        {
            Messenger = messenger;
        }
    }
}