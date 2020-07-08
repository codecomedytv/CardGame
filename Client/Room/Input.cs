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
        private bool Targeting;
        private bool Attacking;
        private Card TargetingCard;
        private Card AttackingCard;

        public Input(Player player)
        {
            User = player;
        }
        
        public void OnCardCreated(Card card)
        {
            object Subscribe(string signal, string method, Card c) => (card.View.Connect(signal, this, method, new Array {c}));
            Subscribe(nameof(CardView.MouseEnteredCard), nameof(OnMouseEnterCard), card);
            Subscribe(nameof(CardView.MouseExitedCard), nameof(OnMouseExitCard), card);
            Subscribe(nameof(CardView.DoubleClicked), nameof(OnCardDoubleClicked), card);
        }
        
        private void OnMouseEnterCard(Card card)
        {
            if (Targeting || Attacking) { return; }
            EmitSignal(nameof(MouseEnteredCard), card);
            if (card.IsInActive || User.IsInActive) { return; }
            if (card.CanTarget)
            {
                card.HighlightTargets();
            }

            else if (card.CanAttack)
            {
                card.HighlightAttackTargets();
            }
        }

        private void OnMouseExitCard(Card card)
        {
            if (Targeting || Attacking) { return; }
            card.StopHighlightingTargets();
        }
        
        private void OnCardDoubleClicked(Card card)
        {
            if (User.State == States.Processing)
            {
                return;
            }
            if (Targeting && User.State == States.Active)
            {
                TargetingCard.StopHighlightingTargets();
                if (TargetingCard.HasTarget(card))
                {
                    EmitSignal(nameof(Activate), TargetingCard, card.Id);
                    User.State = States.Processing;
                    return;
                }
            }

            if (Attacking && User.State == States.Idle)
            {
                card.StopHighlightingAttackTargets();

                if (AttackingCard.HasAttackTarget(card))
                {
                    AttackingCard.AttackUnit(card);
                    User.State = States.Processing;
                    EmitSignal(nameof(Attack), AttackingCard.Id, card.Id);
                }
                else
                {
                    AttackingCard.View.AttackIcon.Visible = false;
                    AttackingCard.View.SelectedTarget.Visible = false;
                }
                Attacking = false;
                AttackingCard = null;
                return;

            }

            if (card.State == CardStates.CanBeDeployed && User.State == States.Idle)
            {
                User.State = States.Processing;
                EmitSignal(nameof(Deploy), card.Id);
            }
            else if (card.State == CardStates.CanBeSet && User.State == States.Idle)
            {
                User.State = States.Processing;
                EmitSignal(nameof(SetFaceDown), card.Id);
            }
            else if (card.State == CardStates.CanBeActivated && !User.IsInActive)
            {
                if (card.CanTarget)
                {
                    // We need a required check as well as a viablity check
                    card.FlipFaceUp();
                    Targeting = true;
                    TargetingCard = card;
                }
                else
                {
                    User.State = States.Processing;
                    EmitSignal(nameof(Activate), card, new Array());
                }
            }
            else if (card.State == CardStates.CanAttack && User.State == States.Idle)
            {
                Attacking = true;
                AttackingCard = card;
                card.View.AttackIcon.Visible = true;
                card.View.SelectedTarget.Visible = true;
            }
        }
    }
}