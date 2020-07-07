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
        
        private readonly CardCatalog CardCatalog;
        private readonly Player User;
        private bool Targeting;
        private bool Attacking;
        private Card TargetingCard;
        private Card AttackingCard;

        public Input(CardCatalog cardCatalog, Player player)
        {
            User = player;
            CardCatalog = cardCatalog;
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
            var playingState = User.State == States.Idle || User.State == States.Active;
            if (card.State == CardStates.Passive || card.State == CardStates.Activated || !playingState)
            {
                return;
            }
            card.View.Legal.Visible = true;
            if (card.Targets())
            {
                foreach (var id in card.ValidTargets)
                {
                    CardCatalog.Fetch(id).View.ValidTarget.Visible = true;
                }
            }

            else if (card.Attacks() && User.State == States.Idle)
            {
                card.View.AttackIcon.Visible = true;
                foreach (var id in card.ValidAttackTargets)
                {
                    CardCatalog.Fetch(id).View.DefenseIcon.Visible = true;
                }
            }
        }

        private void OnMouseExitCard(Card card)
        {
            if (Targeting || Attacking) { return; }
            card.View.Legal.Visible = false;
            card.View.AttackIcon.Visible = false;
            foreach (var id in card.ValidTargets)
            {
                CardCatalog.Fetch(id).View.ValidTarget.Visible = false;
            }

            foreach (var id in card.ValidAttackTargets)
            {
                CardCatalog.Fetch(id).View.DefenseIcon.Visible = false;
            }
        }
        
        private void OnCardDoubleClicked(Card card)
        {
            if (User.State == States.Processing)
            {
                return;
            }
            if (Targeting && User.State == States.Active)
            {
                foreach (var id in TargetingCard.ValidTargets)
                {
                    CardCatalog.Fetch(id).View.ValidTarget.Visible = false;
                }

                if (TargetingCard.ValidTargets.Contains(card.Id))
                {
                    EmitSignal(nameof(Activate), TargetingCard, card.Id);
                    User.State = States.Processing;
                    return;
                }
            }

            if (Attacking && User.State == States.Idle)
            {
                AttackingCard.View.Legal.Visible = false;
                foreach (var id in AttackingCard.ValidAttackTargets)
                {
                    CardCatalog.Fetch(id).View.DefenseIcon.Visible = false;
                }

                if (AttackingCard.ValidAttackTargets.Contains(card.Id))
                {
                    AttackingCard.View.SelectedTarget.Visible = true;
                    AttackingCard.View.AttackIcon.Visible = true;
                    card.View.SelectedTarget.Visible = true;
                    card.View.DefenseIcon.Visible = true;
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
            switch (card.State)
            {
                case CardStates.CanBeDeployed:
                    if (User.State != States.Idle) { return; }
                    card.View.Legal.Visible = false;
                    User.State = States.Processing;
                    EmitSignal(nameof(Deploy), card.Id);
                    break;
                case CardStates.CanBeSet:
                    if (User.State != States.Idle) { return; }
                    card.View.Legal.Visible = false;
                    User.State = States.Processing;
                    EmitSignal(nameof(SetFaceDown), card.Id);
                    break;
                case CardStates.CanBeActivated:
                    if (User.State != States.Idle && User.State != States.Active) { return; }
                    if (card.Targets())
                    {
                        card.FlipFaceUp();
                        Targeting = true;
                        TargetingCard = card;
                        card.View.Legal.Visible = false;
                    }
                    else
                    {
                        card.View.Legal.Visible = false;
                        User.State = States.Processing;
                        EmitSignal(nameof(Activate), card, new Array());
                    }
                    break;
                case CardStates.CanAttack:
                    if (User.State != States.Idle) { return; }
                    Attacking = true;
                    AttackingCard = card;
                    card.View.AttackIcon.Visible = true;
                    card.View.SelectedTarget.Visible = true;
                    break;
                case CardStates.Passive:
                    break;
                case CardStates.Activated:
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}