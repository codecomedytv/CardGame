using System;
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
        
        private Card MousedOverCard;
        public Player User;
        public Opponent Opponent;

        public void Subscribe(Card card)
        {
            card.MouseOvered = OnMousedOverCard;
            card.MouseOveredExit = OnMousedOverExitCard;
        }

        public override void _Input(InputEvent gameEvent)
        {
            if (gameEvent is InputEventMouseButton mb && mb.Doubleclick && MousedOverCard != null)
            {
                OnDoubleClicked(MousedOverCard);
            }
            else if (gameEvent is InputEventMouseButton mob && mob.Doubleclick && User.Attacking)
            {
                User.CardInUse.StopAttacking();
                User.Attacking = false;
                User.CardInUse = null;
                
            }
        }
        
        public void OnMousedOverCard(Card card) => MousedOverCard = card;
        
        public void OnMousedOverExitCard(Card card)
        {
            // Make sure a different card hasn't already overriden it
            MousedOverCard = MousedOverCard == card ? null : MousedOverCard;
        }

        public void OnPassPlayPressed() { if (User.State == States.Active) { PassPlay(); } }

        public void OnEndTurnPressed() { if (User.State == States.Idle) { EndTurn(); } }
        

        private void ChooseAttackTarget(Card card)
        {
            if (User.CardInUse.HasAttackTarget(card))
            {
                User.State = States.Processing;
                card.Defend();
                Attack(User.CardInUse.Id, card.Id);
            }
            else
            {
                User.CardInUse.StopAttacking();
            }

            User.Attacking = false;
            User.CardInUse = null;
        }

        private void OnDoubleClicked(Card card)
        {
            if (User.IsChoosingAttackTarget)
            {
                ChooseAttackTarget(card);
            }
            
            else if (!User.IsInActive)
            {
                TakeAction(card);
            }
            
        }

        private void TakeAction(Card card)
        {
            if (card.CanBeDeployed)
            {
                User.State = States.Processing;
                Deploy(card.Id);
            }
            
            else if (card.CanBeSet)
            {
                User.State = States.Processing;
                SetCard(card.Id);
            }
            
            else if (card.CanBeActivated)
            {
                card.FlipFaceUp();

                // Insert Target Check Here
                User.State = States.Processing;
                Activate(card.Id, 0);
            }
            
            else if (card.CanAttack)
            {
                card.Attack();
                User.Attacking = true;
                User.CardInUse = card;
            }
            
            else if (card.CanAttackDirectly)
            {
                card.Attack();
                Opponent.Defend();
                DirectAttack(card.Id);
            }
            
        }
    }
}