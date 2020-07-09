using System;
using System.Linq;
using CardGame.Client.Cards;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Room
{
    public class Input: Godot.Node
    {
        [Signal]
        public delegate void MouseEnteredCard();
        
        [Signal]
        public delegate void Deploy();

        [Signal]
        public delegate void SetFaceDown();

        [Signal]
        public delegate void Activate();

        [Signal]
        public delegate void Attack();
        
        private readonly Player User;
        

        public Input(Player player)
        {
            User = player;
        }
        
        public void OnCardCreated(Card card)
        {
            card.View.Connect(nameof(CardView.DoubleClicked), this, nameof(OnCardDoubleClicked), new Array {card});
        }
        
        private void OnCardDoubleClicked(Card card)
        {
            if (User.State == States.Processing)
            {
                return;
            }
            
            // Begin Method?
            if (User.Targeting && User.State == States.Active)
            {
                if (User.TargetingCard.HasTarget(card))
                {
                    EmitSignal(nameof(Activate), User.TargetingCard, card.Id);
                    User.State = States.Processing;
                    return;
                }
            }

            if (User.Attacking && User.State == States.Idle)
            {
                if (User.AttackingCard.HasAttackTarget(card))
                {
                    User.AttackingCard.AttackUnit(card);
                    User.State = States.Processing;
                    EmitSignal(nameof(Attack), User.AttackingCard.Id, card.Id);
                }
                else
                {
                    User.AttackingCard.CancelAttack();
                }
                User.Attacking = false;
                User.AttackingCard = null;
                return;
            }

            // Begin Method?
            if (card.CanBeDeployed)
            {
                User.State = States.Processing;
                EmitSignal(nameof(Deploy), card.Id);
            }
            else if (card.CanBeSet)
            {
                User.State = States.Processing;
                EmitSignal(nameof(SetFaceDown), card.Id);
            }
            else if (card.CanBeActivated)
            {
                card.FlipFaceUp();

                // We're checking if it can target, but it seems our fallback (the else) is to activate it without
                // selecting a target which should (in most cases at least) be an illegal play.
                
                // In retrospect this is okay because if the card required targets but had none valid, it wouldn't
                // be able to satisfy this condition since its state wouldn't be CanBeActivated.
                if (card.CanTarget)
                {
                    // We return our of this so players have a chance to select the target before activation
                    User.Targeting = true;
                    User.TargetingCard = card;
                }
                else
                {
                    User.State = States.Processing;
                    EmitSignal(nameof(Activate), card, new Array());
                }
            }
            else if (card.CanAttack)
            {
                User.Attacking = true;
                User.AttackingCard = card;
                card.Select();
            }
        }
    }
}