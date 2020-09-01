using System;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Messenger: Node
    {
        private const int ServerId = 1;
        private int Id => Multiplayer.GetNetworkUniqueId();
        public Messenger() { Name = "Messenger"; }
        public void SetReady() => RpcId(ServerId, "SetReady", Id);

        public Action<States> ExecuteEvents; 
        public Action<Commands, object[]> QueueEvent;
        
        [Puppet]
        // Change stateIntoACommand?
        private void Execute(States stateAfterExecution) => ExecuteEvents(stateAfterExecution);

        [Puppet]
        private void Queue(Commands command, params object[] args) => QueueEvent(command, args);
        
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