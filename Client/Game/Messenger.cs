using Godot;

namespace CardGame.Client.Game
{
    public class Messenger: Node
    {
        private const int ServerId = 1;
        private int Id => Multiplayer.GetNetworkUniqueId();
        public readonly MessageReceiver Receiver;
        public readonly MessageSender Sender;

        public Messenger()
        {
            Name = "Messenger";
            Sender = new MessageSender(this);
            Receiver = new MessageReceiver();
        }
        
        public void SetReady() => RpcId(ServerId, "SetReady", Id);

        [Puppet]
        public void Execute(int stateAfterExecution) => Receiver.Execute(stateAfterExecution);

        [Puppet]
        public void Queue(string signal, params object[] args) => Receiver.Queue(signal, args);
    }
}