﻿using CardGame.Client.Library.Cards;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Game
{
    public class Messenger: Control
    {
        [Signal]
        public delegate void Retry();

        [Signal]
        public delegate void QueuedEvent();

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
        public void QueueEvent(Dictionary<string, int> message)
        {
            EmitSignal(nameof(QueuedEvent), message);
        }

        [Puppet]
        public void ExecuteEvents()
        {
            EmitSignal(nameof(ExecutedEvents));
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

        public void Activate(Card card, Array targets)
        {
            RpcId(ServerId, "Activate", Id, card.Id, targets);
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