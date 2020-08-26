using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Game
{
    public class GameInput: Node
    {
        [Signal]
        public delegate void Deploy();

        [Signal]
        public delegate void SetCard();

        [Signal]
        public delegate void Activate();

        [Signal]
        public delegate void PassPlay();

        [Signal]
        public delegate void Attack();

        [Signal]
        public delegate void EndTurn();
        
        private Card MousedOverCard;
        public Player User;

        public override void _Input(InputEvent gameEvent)
        {
            if (gameEvent is InputEventMouseButton mb && mb.Doubleclick && MousedOverCard != null)
            {
                GD.Print($"Double Clicked {MousedOverCard.Id}");
                OnDoubleClicked(MousedOverCard);
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
            GD.Print("pressed end turn");
            if (User.State == States.Idle)
            {
                GD.Print("Ending Turn");
                EmitSignal(nameof(EndTurn));
            }
        }

        public void OnPassPlayPressed()
        {
            GD.Print("pressed pass play turn");
            if (User.State == States.Active)
            {
                GD.Print("passing play");
                EmitSignal(nameof(PassPlay));
            }
        }

        private void ChooseAttackTarget(Card card)
        {
            if (User.CardInUse.HasAttackTarget(card))
            {
                User.State = States.Processing;
                card.DefendingIcon.Visible = true;
                EmitSignal(nameof(Attack), User.CardInUse.Id, card.Id);
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
                EmitSignal(nameof(Deploy), card.Id);
            }
            
            else if (card.CanBeSet)
            {
                User.State = States.Processing;
                EmitSignal(nameof(SetCard), card.Id);
            }
            
            else if (card.CanBeActivated)
            {
                card.FlipFaceUp();

                // Insert Target Check Here
                User.State = States.Processing;
                EmitSignal(nameof(Activate), card.Id, new Array());

            }
            
            else if (card.CanAttack)
            {
                card.AttackingIcon.Visible = true;
                User.Attacking = true;
                User.CardInUse = card;
            }
            
        }
    }
}