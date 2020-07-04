﻿using System.Collections.Generic;
using CardGame.Client.Library.Cards;
using Godot;
using Godot.Collections;
using States = CardGame.Client.Room.States;

namespace CardGame.Client.Room
{
    public class Messenger: Control
    {
        [Signal] public delegate void DeckLoaded();
        
        [Signal] public delegate void Disqualified();

        [Signal] public delegate void DrawQueued();

        [Signal] public delegate void DeployQueued();

        [Signal] public delegate void SetFaceDownQueued();

        [Signal] public delegate void ActivationQueued();

        [Signal] public delegate void TriggerQueued();

        [Signal] public delegate void CardStateSet();

        [Signal] public delegate void ValidTargetsSet();

        [Signal] public delegate void ValidAttackTargetsSet();

        [Signal] public delegate void CardDestroyed();
        
        [Signal] public delegate void UnitBattled();

        [Signal] public delegate void ExecutedEvents();

        [Signal] public delegate void DisconnectPlayer();

        [Signal] public delegate void CardSentToZone();

        [Signal] public delegate void LifeLost();

        private const int ServerId = 1;
        public int Id = 0;

        public Messenger() =>Name = "Messenger";
        
        public void SetReady() => RpcId(ServerId, "SetReady", Id); // Complains about no network peer being assigned?

        [Puppet] public void Disqualify() => EmitSignal(nameof(Disqualified));
        [Puppet] public void ExecuteEvents(States stateAfterExecution) => EmitSignal(nameof(ExecutedEvents), stateAfterExecution);
        
        [Puppet] public void LoadDeck(Godot.Collections.Dictionary<int, SetCodes> deck) => EmitSignal(nameof(DeckLoaded), deck);
        
        [Puppet] public void Draw(int id, bool isOpponent) => EmitSignal(nameof(DrawQueued), id, isOpponent);
        
        [Puppet] public void QueueDeploy(int id) => EmitSignal(nameof(DeployQueued), id);

        [Puppet] public void QueueDeploy(int id, SetCodes setCode) => EmitSignal(nameof(DeployQueued), id, setCode);

        [Puppet] public void QueueSetFaceDown(int id, bool isOpponent) => EmitSignal(nameof(SetFaceDownQueued), id, isOpponent);
        
        [Puppet] public void Activation(int id, int positionInLink) => EmitSignal(nameof(ActivationQueued), id, positionInLink);

        [Puppet] public void Activation(int id, SetCodes setCode, int positionInLink) => EmitSignal(nameof(ActivationQueued), id, setCode, positionInLink);

        [Puppet] public void Trigger(int id, int positionInLink) => EmitSignal(nameof(TriggerQueued), id, positionInLink);
        
        [Puppet] public void SetCardState(int id, CardStates states) => EmitSignal(nameof(CardStateSet), id, states);

        [Puppet] public void SetValidTargets(int id, List<int> validTargets) => EmitSignal(nameof(ValidTargetsSet), id, validTargets);

        [Puppet] public void SetValidAttackTargets(int id, List<int> validAttackTargets) => EmitSignal(nameof(ValidAttackTargetsSet), id, validAttackTargets); 
        
        [Puppet] public void BattleUnit(int attackerId, int defenderId, bool isOpponentAttacking) => EmitSignal(nameof(UnitBattled), attackerId, defenderId, isOpponentAttacking);

        [Puppet] public void ForceDisconnected(int reason) => EmitSignal(nameof(DisconnectPlayer), reason);

        [Puppet] public void SentToZone(int cardId, ZoneIds zoneId) =>EmitSignal(nameof(CardSentToZone), cardId, zoneId);

        [Puppet] public void LoseLife(int lifeLost, bool isOpponent) => EmitSignal(nameof(LifeLost), lifeLost, isOpponent);

        public void OnDeployDeclared(int cardId) => RpcId(ServerId, "Deploy", Id, cardId);

        public void OnAttackDeclared(int attackerId, int cardId) => RpcId(ServerId, "Attack", Id, attackerId, cardId);
        
        public void OnSetFaceDownDeclared(int cardId) => RpcId(ServerId, "SetFaceDown", Id, cardId);

        public void OnActivationDeclared(Card card, int targetId) => RpcId(ServerId, "Activate", Id, card.Id, targetId);
        
        public void Target(int cardId) => RpcId(ServerId, "Target", Id, cardId);
        
        public void OnPassPriorityDeclared() => RpcId(ServerId, "PassPlay", Id);
        
        public void OnEndTurnDeclared() => RpcId(ServerId, "EndTurn", Id);
        
    }
}