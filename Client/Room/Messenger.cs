using System.Collections.Generic;
using CardGame.Client.Library.Cards;
using Godot;
using Godot.Collections;
using States = CardGame.Client.Room.States;

namespace CardGame.Client.Room
{
    public class Messenger: Control
    {
        [Signal] public delegate void CardUpdated();
        [Signal] public delegate void DeckLoaded();
        
        [Signal] public delegate void Disqualified();

        [Signal] public delegate void DrawQueued();

        [Signal] public delegate void DeployQueued();

        [Signal] public delegate void SetFaceDownQueued();

        [Signal] public delegate void ActivationQueued();

        [Signal] public delegate void TriggerQueued();
        
        [Signal] public delegate void CardDestroyed();
        
        [Signal] public delegate void UnitBattled();

        [Signal] public delegate void ExecutedEvents();

        [Signal] public delegate void DisconnectPlayer();

        [Signal] public delegate void CardSentToZone();

        [Signal] public delegate void LifeLost();

        private const int ServerId = 1;
        public int Id = 0;

        public Messenger() => Name = "Messenger";
        
        public void SetReady() => RpcId(ServerId, "SetReady", Id); // Complains about no network peer being assigned?

        [Puppet] public void Disqualify() => EmitSignal(nameof(Disqualified));
        [Puppet] public void ExecuteEvents(States stateAfterExecution) => EmitSignal(nameof(ExecutedEvents), stateAfterExecution);
        [Puppet] public void UpdateCard(params object[] args) => EmitSignal(nameof(CardUpdated), args);
        [Puppet] public void LoadDeck(params object[] args) => EmitSignal(nameof(DeckLoaded), args);
        [Puppet] public void Draw(params object[] args) => EmitSignal(nameof(DrawQueued), args);
        [Puppet] public void QueueDeploy(params object[] args) => EmitSignal(nameof(DeployQueued), args);
        [Puppet] public void QueueSetFaceDown(params object[] args) => EmitSignal(nameof(SetFaceDownQueued), args);
        [Puppet] public void Activation(params object[] args) { EmitSignal(nameof(ActivationQueued), args); }
        [Puppet] public void Trigger(params object[] args) => EmitSignal(nameof(TriggerQueued), args);
        [Puppet] public void BattleUnit(params object[] args) => EmitSignal(nameof(UnitBattled), args);
        [Puppet] public void ForceDisconnected(int reason) => EmitSignal(nameof(DisconnectPlayer), reason);
        [Puppet] public void SentToZone(params object[] args) =>EmitSignal(nameof(CardSentToZone), args);
        [Puppet] public void LoseLife(params object[] args) => EmitSignal(nameof(LifeLost), args);

        public void DeclareDeploy(int cardId) => RpcId(ServerId, "OnDeployDeclared", Id, cardId);

        public void DeclareAttack(int attackerId, int cardId) => RpcId(ServerId, "OnAttackDeclared", Id, attackerId, cardId);
        
        public void DeclareSetFaceDown(int cardId) => RpcId(ServerId, "OnSetFaceDownDeclared", Id, cardId);

        public void DeclareActivation(Card card, int targetId) => RpcId(ServerId, "OnActivationDeclared", Id, card.Id, targetId);
        
        public void DeclareTarget(int cardId) => RpcId(ServerId, "OnTargetDeclared", Id, cardId);
        
        public void DeclarePassPlay() => RpcId(ServerId, "OnPassPlayDeclared", Id);
        
        public void DeclareEndTurn() => RpcId(ServerId, "OnEndTurnDeclared", Id);
        
    }
}