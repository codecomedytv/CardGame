﻿using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Game
{
    public class GameInput: Node
    {
        public Action<int> Deploy;
        public Action<int> SetCard;
        public Action<int, int> Activate;
        public Action<int, int> Attack;
        public Action<int> DirectAttack;
        public Action PassPlay;
        public Action EndTurn;

        public Action<Card> AttackerSelected;
        public Action<Card> DefenderSelected;
        public Action<BasePlayer> AttackedDirectly;
        public Action<Card> AttackStopped;

        private Card MousedOverCard;
        public Player User;
        public Opponent Opponent;

        public void Subscribe(Card card)
        {
            card.MouseOvered = OnMousedOverCard;
            card.MouseOveredExit = OnMousedOverExitCard;
        }

        public void OnCardCreated(Card card)
        {
            card.MouseOvered += OnMousedOverCard;
            card.MouseOveredExit = OnMousedOverExitCard;
        }

        public override void _Input(InputEvent gameEvent)
        {
            if (gameEvent is InputEventMouseButton mb && mb.Doubleclick && MousedOverCard != null)
            {
                OnDoubleClicked(MousedOverCard);
            }
            else if (gameEvent is InputEventMouseButton mob && mob.Doubleclick && User.PlayerState.Attacking)
            {
                AttackStopped?.Invoke(User.PlayerState.CardInUse);
                User.PlayerState.Attacking = false;
                User.PlayerState.CardInUse = null;
                
            }
        }
        
        public void OnMousedOverCard(Card card) => MousedOverCard = card;
        
        public void OnMousedOverExitCard(Card card)
        {
            // Make sure a different card hasn't already overriden it
            MousedOverCard = MousedOverCard == card ? null : MousedOverCard;
        }

        public void OnPassPlayPressed() { if (User.PlayerState.State == States.Active) { PassPlay(); } }

        public void OnEndTurnPressed() { if (User.PlayerState.State == States.Idle) { EndTurn(); } }
        

        private void ChooseAttackTarget(Card card)
        {
            if (User.PlayerState.CardInUse.HasAttackTarget(card))
            {
                User.PlayerState.State = States.Processing;
                DefenderSelected?.Invoke(card);
                Attack(User.PlayerState.CardInUse.Id, card.Id);
            }
            else
            {
                AttackStopped?.Invoke(User.PlayerState.CardInUse);
            }

            card.ValidAttackTargets.StopHighlighting();
            User.PlayerState.Attacking = false;
            User.PlayerState.CardInUse = null;
        }

        private void OnDoubleClicked(Card card)
        {
            if (User.PlayerState.IsChoosingAttackTarget)
            {
                ChooseAttackTarget(card);
            }
            
            else if (!User.PlayerState.IsInActive)
            {
                TakeAction(card);
            }
            
        }

        private void TakeAction(Card card)
        {
            if (card.CanBeDeployed)
            {
                User.PlayerState.State = States.Processing;
                Deploy(card.Id);
            }
            
            else if (card.CanBeSet)
            {
                User.PlayerState.State = States.Processing;
                SetCard(card.Id);
            }
            
            else if (card.CanBeActivated)
            {
                card.FlipFaceUp();

                // Insert Target Check Here
                User.PlayerState.State = States.Processing;
                Activate(card.Id, 0);
            }
            
            else if (card.CanAttack)
            {
                AttackerSelected?.Invoke(card);
                card.ValidAttackTargets.Highlight();
                User.PlayerState.Attacking = true;
                User.PlayerState.CardInUse = card;
            }
            
            else if (card.CanAttackDirectly)
            {
                AttackerSelected?.Invoke(card);
                AttackedDirectly?.Invoke(Opponent);
                DirectAttack(card.Id);
            }
            
        }
    }
}