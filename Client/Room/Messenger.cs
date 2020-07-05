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

        [Puppet]
        public void UpdateCard(int id, CardStates state, IEnumerable<int> attackTargets, IEnumerable<int> targets)
        {
            EmitSignal(nameof(CardUpdated), id, state, attackTargets, targets);
        }
        
        [Puppet] public void LoadDeck(Godot.Collections.Dictionary<int, SetCodes> deck) => EmitSignal(nameof(DeckLoaded), deck);
        
        [Puppet] public void Draw(int id, bool isOpponent) => EmitSignal(nameof(DrawQueued), id, isOpponent);
        
        [Puppet] public void QueueDeploy(int id, SetCodes setCode, bool isOpponent) => EmitSignal(nameof(DeployQueued), id, setCode, isOpponent);

       // [Puppet] public void QueueDeploy(int id, SetCodes setCode) => EmitSignal(nameof(DeployQueued), id, setCode);

        [Puppet] public void QueueSetFaceDown(int id, bool isOpponent) => EmitSignal(nameof(SetFaceDownQueued), id, isOpponent);
        
        [Puppet] public void Activation(int id, int positionInLink) => EmitSignal(nameof(ActivationQueued), id, positionInLink);

        [Puppet] public void Activation(int id, SetCodes setCode, int positionInLink) => EmitSignal(nameof(ActivationQueued), id, setCode, positionInLink);

        [Puppet] public void Trigger(int id, int positionInLink) => EmitSignal(nameof(TriggerQueued), id, positionInLink);
        
        [Puppet] public void BattleUnit(int attackerId, int defenderId, bool isOpponentAttacking) => EmitSignal(nameof(UnitBattled), attackerId, defenderId, isOpponentAttacking);

        [Puppet] public void ForceDisconnected(int reason) => EmitSignal(nameof(DisconnectPlayer), reason);

        [Puppet] public void SentToZone(int cardId, ZoneIds zoneId) =>EmitSignal(nameof(CardSentToZone), cardId, zoneId);

        [Puppet] public void LoseLife(int lifeLost, bool isOpponent) => EmitSignal(nameof(LifeLost), lifeLost, isOpponent);

        public void DeclareDeploy(int cardId) => RpcId(ServerId, "OnDeployDeclared", Id, cardId);

        public void DeclareAttack(int attackerId, int cardId) => RpcId(ServerId, "OnAttackDeclared", Id, attackerId, cardId);
        
        public void DeclareSetFaceDown(int cardId) => RpcId(ServerId, "OnSetFaceDownDeclared", Id, cardId);

        public void DeclareActivation(Card card, int targetId) => RpcId(ServerId, "OnActivationDeclared", Id, card.Id, targetId);
        
        public void DeclareTarget(int cardId) => RpcId(ServerId, "OnTargetDeclared", Id, cardId);
        
        public void DeclarePassPlay() => RpcId(ServerId, "OnPassPlayDeclared", Id);
        
        public void DeclareEndTurn() => RpcId(ServerId, "OnEndTurnDeclared", Id);
        
    }
}