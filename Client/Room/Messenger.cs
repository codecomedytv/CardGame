using System;
using System.Collections.Generic;
using CardGame.Client.Cards;
using Godot;
using Godot.Collections;
using States = CardGame.Client.Room.States;

namespace CardGame.Client.Room
{
    public class Messenger: Control
    {
        [Signal] public delegate void UpdateCard();
        [Signal] public delegate void RevealCard();
        [Signal] public delegate void LoadDeck();
        [Signal] public delegate void OpponentAttackUnit();
        [Signal] public delegate void Disqualified();
        [Signal] public delegate void Draw();
        [Signal] public delegate void Deploy();
        [Signal] public delegate void SetFaceDown();
        [Signal] public delegate void Activate();
        [Signal] public delegate void Trigger();
        [Signal] public delegate void BattleUnit();
        [Signal] public delegate void ExecutedEvents();
        [Signal] public delegate void SendCardToZone();
        [Signal] public delegate void LoseLife();

        private const int ServerId = 1;
        private int Id => Multiplayer.GetNetworkUniqueId();

        public Messenger() { Name = "Messenger"; }

        public void SubscribeTo(Input input)
        {
            object Subscribe(string signal, string method) => (input.Connect(signal, this, method));
            Subscribe(nameof(Input.Deploy), nameof(DeclareDeploy));
            Subscribe(nameof(Input.SetFaceDown), nameof(DeclareSetFaceDown));
            Subscribe(nameof(Input.Activate), nameof(DeclareActivation));
            Subscribe(nameof(Input.Attack),  nameof(DeclareAttack));
        }
        public void SetReady() => RpcId(ServerId, "SetReady", Id);
        [Puppet] public void Disqualify() => EmitSignal(nameof(Disqualified));
        [Puppet] public void Execute(States stateAfterExecution) => EmitSignal(nameof(ExecutedEvents), stateAfterExecution);
        [Puppet] public void Queue(string signal, params object[] args) => EmitSignal(signal, args);

        public void DeclareDeploy(int cardId) => RpcId(ServerId, "OnDeployDeclared", Id, cardId);

        public void DeclareAttack(int attackerId, int cardId) => RpcId(ServerId, "OnAttackDeclared", Id, attackerId, cardId);
        
        public void DeclareSetFaceDown(int cardId) => RpcId(ServerId, "OnSetFaceDownDeclared", Id, cardId);

        public void DeclareActivation(Card card, int targetId) => RpcId(ServerId, "OnActivationDeclared", Id, card.Id, targetId);
        
        public void DeclareTarget(int cardId) => RpcId(ServerId, "OnTargetDeclared", Id, cardId);
        
        public void DeclarePassPlay() => RpcId(ServerId, "OnPassPlayDeclared", Id);
        
        public void DeclareEndTurn() => RpcId(ServerId, "OnEndTurnDeclared", Id);
        
    }
}