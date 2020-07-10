using System;
using System.Linq;
using CardGame.Client.Cards;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Room
{
    public class Input : Godot.Node
    {
        [Signal]
        public delegate void Deploy();

        [Signal]
        public delegate void SetFaceDown();

        [Signal]
        public delegate void Activate();

        [Signal]
        public delegate void Attack();

        private readonly Player User;
        public bool NoCardsWereClicked(Vector2 pos)
        {
            return !GetTree().GetNodesInGroup("cards").Cast<Card>().Any(c => WasClicked(c, pos));
        }
		
        private bool WasClicked(Card card, Vector2 pos) => card.GetGlobalRect().HasPoint(pos);

        public Input(Player player) => User = player;

        public void OnCardCreated(Card card)
        {
            card.View.Connect(nameof(CardView.DoubleClicked), this, nameof(OnCardDoubleClicked), new Array {card});
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent is InputEventMouseButton mouseButton && mouseButton.Doubleclick)
            {
                if (NoCardsWereClicked(mouseButton.Position) && User.Attacking)
                {
                    User.CardInUse.Deselect();
                    User.CardInUse = null;
                    User.Attacking = false;
                }
            }
        }
        
        private void OnCardDoubleClicked(Card card)
        {
            if (User.State == States.Processing)
            {
                return;
            }
            
            // User State Checks May Be Pointless? Sure we only only be targeting at this point?
            if (User.Targeting && User.State == States.Active)
            {
                ChooseEffectTarget(card);
            }

            if (User.Attacking && User.State == States.Idle)
            {
                ChooseAttackTarget(card);
                return;
            }

            TakeAction(card);
        }

        private void ChooseEffectTarget(Card card)
        {
            if (!User.CardInUse.HasTarget(card)) return;
            EmitSignal(nameof(Activate), User.CardInUse, card.Id);
            User.State = States.Processing;
        }

        private void ChooseAttackTarget(Card card)
        {
            if (User.CardInUse.HasAttackTarget(card))
            {
                User.CardInUse.AttackUnit(card);
                User.State = States.Processing;
                EmitSignal(nameof(Attack), User.CardInUse.Id, card.Id);
            }
            else
            {
                User.CardInUse.CancelAttack();
            }

            User.Attacking = false;
            User.CardInUse = null;
        }

        private void TakeAction(Card card) 
        {
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

            else if (card.CanAttack)
            {
                User.Attacking = true;
                User.CardInUse = card;
                card.Select();
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
                    User.CardInUse = card;
                }
                else
                {
                    User.State = States.Processing;
                    EmitSignal(nameof(Activate), card, new Array());
                } 
            }
        }
    }
}