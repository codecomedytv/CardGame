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

        public override void _Input(InputEvent gameEvent)
        {
            if (gameEvent is InputEventMouseButton mb && mb.Doubleclick && MousedOverCard != null)
            {
                GD.Print($"Double Clicked {MousedOverCard.Id}");
                OnDoubleClicked(MousedOverCard);
            }
            else if (gameEvent is InputEventMouseButton mob && mob.Doubleclick && User.Attacking)
            {
                User.CardInUse.AttackingIcon.Visible = false;
                User.Attacking = false;
                User.CardInUse = null;
                
            }
        }

        public void SubscribeTo(Card card)
        {
            card.Connect(nameof(Card.MouseOvered), this, nameof(OnMousedOverCard));
            card.Connect(nameof(Card.MouseOveredExit), this, nameof(OnMousedOverExitCard));
        }

        private void OnMousedOverCard(Card card)
        {
            MousedOverCard = card;
        }

        private void OnMousedOverExitCard(Card card)
        {
            // Make sure a different card hasn't already overriden it
            if (MousedOverCard == card)
            {
                MousedOverCard = null;
            }
        }

        public void OnEndTurnPressed()
        {
            if (User.State == States.Idle)
            {
                EndTurn();
            }
        }


        public void OnPassPlayPressed()
        {
            if (User.State == States.Active)
            {
                PassPlay();
            }
        }

        private void ChooseAttackTarget(Card card)
        {
            if (User.CardInUse.HasAttackTarget(card))
            {
                User.State = States.Processing;
                card.DefendingIcon.Visible = true;
                Attack(User.CardInUse.Id, card.Id);
            }
            else
            {
                User.CardInUse.AttackingIcon.Visible = false;
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
                card.AttackingIcon.Visible = true;
                User.Attacking = true;
                User.CardInUse = card;
            }
            
            else if (card.CanAttackDirectly)
            {
                card.AttackingIcon.Visible = true;
                Opponent.Defend();
                DirectAttack(card.Id);
            }
            
        }
    }
}