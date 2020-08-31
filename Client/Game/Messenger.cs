using Godot;

namespace CardGame.Client.Game
{
    public class Messenger: Node
    {
        private const int ServerId = 1;
        private int Id => Multiplayer.GetNetworkUniqueId();

        public Messenger() { Name = "Messenger"; }
        
        public void SetReady() => RpcId(ServerId, "SetReady", Id);
        
        [Signal] public delegate void ExecutedEvents();
        [Signal] public delegate void QueueEvents();

        [Puppet]
        public void Execute(int stateAfterExecution)
        {
            EmitSignal(nameof(ExecutedEvents), stateAfterExecution);
        }

        [Puppet]
        public void Queue(Commands command, params object[] args)
        {
            EmitSignal(nameof(QueueEvents), command, args);
        }
        
        public void DeclareDeploy(int cardId)
        {
            RpcId(ServerId, "OnDeployDeclared", Id, cardId);
        }

        public void DeclareSet(int cardId)
        {
            RpcId(ServerId, "OnSetFaceDownDeclared", Id, cardId);
        }

        public void DeclareAttack(int attackerId, int cardId)
        {
            RpcId(ServerId, "OnAttackDeclared", Id, attackerId, cardId);
        }

        public void DeclareDirectAttack(int attackerId)
        {
            RpcId(ServerId, "OnDirectAttackDeclared", Id, attackerId);
        }

        public void DeclareActivation(int cardId, int targetId)
        {
            RpcId(ServerId, "OnActivationDeclared", Id, cardId, targetId);
        }

        public void DeclarePassPlay()
        {
            RpcId(ServerId, "OnPassPlayDeclared", Id);
        }

        public void DeclareEndTurn()
        {
            RpcId(ServerId, "OnEndTurnDeclared", Id);
        }
    }
}