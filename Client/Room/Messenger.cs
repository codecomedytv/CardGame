using System.Collections.Generic;
using CardGame.Client.Library.Cards;
using Godot;
using Godot.Collections;
using States = CardGame.Client.Players.States;

namespace CardGame.Client.Room
{
    public class Messenger: Control
    {
        [Signal]
        public delegate void Retry();

        [Signal]
        public delegate void QueuedEvent();

        [Signal]
        public delegate void DeckLoaded();
        
        [Signal]
        public delegate void Disqualified();

        [Signal]
        public delegate void DrawQueued();

        [Signal]
        public delegate void DeployQueued();

        [Signal]
        public delegate void SetFaceDownQueued();

        [Signal]
        public delegate void ActivationQueued();

        [Signal]
        public delegate void TriggerQueued();

        [Signal]
        public delegate void CardStateSet();

        [Signal]
        public delegate void ValidTargetsSet();

        [Signal]
        public delegate void ValidAttackTargetsSet();

        [Signal]
        public delegate void CardDestroyed();

        [Signal]
        public delegate void ExecutedEvents();

        [Signal]
        public delegate void DisconnectPlayer();

        private const int ServerId = 1;
        public int Id = 0;

        public Messenger()
        {
            Name = "Messenger";
        }

        public void SetReady()
        {
            RpcId(ServerId, "SetReady", Id); // Complains about no network peer being assigned?
        }

        [Puppet]
        public void Disqualify()
        {
            EmitSignal(nameof(Disqualified));
        }
        
        [Puppet]
        public void ExecuteEvents(States stateAfterExecution)
        {
            EmitSignal(nameof(ExecutedEvents), stateAfterExecution);
        }

        [Puppet]
        public void QueueEvent(Godot.Collections.Dictionary<string, int> message)
        {
            EmitSignal(nameof(QueuedEvent), message);
        }

        [Puppet]
        public void LoadDeck(Godot.Collections.Dictionary<int, SetCodes> deck)
        {
            EmitSignal(nameof(DeckLoaded), deck);
        }

        [Puppet]
        public void QueueDraw(int id, SetCodes setCode)
        {
            EmitSignal(nameof(DrawQueued), id, setCode);
        }

        [Puppet]
        public void QueueDraw()
        {
            EmitSignal(nameof(DrawQueued));
        }

        [Puppet]
        public void QueueDeploy(int id)
        {
            EmitSignal(nameof(DeployQueued), id);
        }

        [Puppet]
        public void QueueDeploy(int id, SetCodes setCode)
        {
            EmitSignal(nameof(DeployQueued), id, setCode);
        }

        [Puppet]
        public void QueueSetFaceDown(int id)
        {
            EmitSignal(nameof(SetFaceDownQueued), id);
        }
        
        [Puppet]
        public void QueueSetFaceDown()
        {
            EmitSignal(nameof(SetFaceDownQueued));
        }

        [Puppet]
        public void QueueActivation(int id, int positionInLink)
        {
            EmitSignal(nameof(ActivationQueued), id, positionInLink);
        }

        [Puppet]
        public void QueueActivation(int id, SetCodes setCode, int positionInLink)
        {
            EmitSignal(nameof(ActivationQueued), id, setCode, positionInLink);
        }

        [Puppet]
        public void QueueTrigger(int id, int positionInLink)
        {
            EmitSignal(nameof(TriggerQueued), id, positionInLink);
        }

        [Puppet]
        public void SetCardState(int id, CardStates states)
        {
            EmitSignal(nameof(CardStateSet), id, states);
        }

        [Puppet]
        public void SetValidTargets(int id, List<int> validTargets)
        {
            EmitSignal(nameof(ValidTargetsSet), id, validTargets);
        }

        [Puppet]
        public void SetValidAttackTargets(int id, List<int> validAttackTargets)
        {
            EmitSignal(nameof(ValidAttackTargetsSet), id, validAttackTargets);
        }

        [Puppet]
        public void DestroyCard(int id)
        {
            EmitSignal(nameof(CardDestroyed), id);
        }

        [Puppet]
        public void ForceDisconnected(int reason)
        {
            EmitSignal(nameof(DisconnectPlayer), reason);
        }
        public void Deploy(int cardId)
        {
            RpcId(ServerId, "Deploy", Id, cardId);
        }

        public void Attack(int attackerId, int cardId)
        {
            RpcId(ServerId, "Attack", Id, attackerId, cardId);
        }


        public void SetFaceDown(int cardId)
        {
            RpcId(ServerId, "SetFaceDown", Id, cardId);
        }

        public void Activate(Card card, int targetId)
        {
             RpcId(ServerId, "Activate", Id, card.Id, targetId);
        }

        public void Target(int cardId)
        {
            RpcId(ServerId, "Target", Id, cardId);
        }


        public void PassPriority()
        {
            RpcId(ServerId, "PassPlay", Id);
        }

        public void EndTurn()
        {
            RpcId(ServerId, "EndTurn", Id);
        }
        
    }
}