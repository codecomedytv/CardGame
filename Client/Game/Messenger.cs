using Godot;

namespace CardGame.Client.Game
{
    public class Messenger: Node
    {
        private const int ServerId = 1;
        private int Id => Multiplayer.GetNetworkUniqueId();
        
        // Experiment With Signals Unpacking
        // See if we can do it more directly or straightforward
        [Signal] public delegate void ExecutedEvents();
        [Signal] public delegate void Draw();
        [Signal] public delegate void LoadDeck();
        [Signal] public delegate void UpdateCard();
        public Messenger() => Name = "Messenger";
        public void SetReady() => RpcId(ServerId, "SetReady", Id);
        [Puppet] public void Execute(int stateAfterExecution) => EmitSignal(nameof(ExecutedEvents), stateAfterExecution);
        [Puppet] public void Queue(string signal, params object[] args) => EmitSignal(signal, args);
    }
}