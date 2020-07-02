using System;
using System.Linq;
using CardGame.Client.Library.Cards;
using CardGame.Client.Players;
using Godot;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Client.Room
{
    public class CardCatalog: Object
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
        
        private readonly System.Collections.Generic.Dictionary<int, Card> CardsById = new System.Collections.Generic.Dictionary<int, Card>();
        public Player User;
        private bool Targeting = false;
        private bool Attacking = false;
        private bool IsActing;
        private Card TargetingCard;
        private Card AttackingCard;

        // We could possibly set the state directly?
        public void OnStateSet()
        {
           IsActing = false;
        }

        public void RegisterCard(Card card)
        {
            CardsById[card.Id] = card;
        }

        public Card this[int id]
        {
            get => CardsById[id];
            set => CardsById[id] = AddCard(value);
        }
        
        private Card AddCard(Card card)
        {
            card.Connect(nameof(Card.MouseEnteredCard), this, nameof(OnMouseEnterCard), new Array {card});
            card.Connect(nameof(Card.MouseExitedCard), this, nameof(OnMouseExitCard), new Array {card});
            card.Connect(nameof(Card.DoubleClicked), this, nameof(OnCardDoubleClicked), new Array {card});
            return card;
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
            card.Legal.Visible = true;
            if (card.Targets())
            {
                foreach (var id in card.ValidTargets)
                {
                    CardsById[id].ValidTarget.Visible = true;
                }
            }

            else if (card.Attacks() && User.State == States.Idle)
            {
                card.AttackIcon.Visible = true;
                foreach (var id in card.ValidAttackTargets)
                {
                    CardsById[id].DefenseIcon.Visible = true;
                }
            }
        }

        private void OnMouseExitCard(Card card)
        {
            if (Targeting || Attacking) { return; }
            card.Legal.Visible = false;
            card.AttackIcon.Visible = false;
            foreach (var id in card.ValidTargets)
            {
                CardsById[id].ValidTarget.Visible = false;
            }

            foreach (var id in card.ValidAttackTargets)
            {
                CardsById[id].DefenseIcon.Visible = false;
            }
        }
        
        private void OnCardDoubleClicked(Card card)
        {
            if (IsActing)
            {
                // We want to prevent sending the same action to the server twice
                // So we have a callback boolean we check to exit early
                return;
                
            }
            if (Targeting && User.State == States.Active)
            {
                foreach (var id in TargetingCard.ValidTargets)
                {
                    CardsById[id].ValidTarget.Visible = false;
                }

                if (TargetingCard.ValidTargets.Contains(card.Id))
                {
                    EmitSignal(nameof(Activate), TargetingCard, card.Id);
                    IsActing = true;
                    return;
                }
            }

            if (Attacking && User.State == States.Idle)
            {
                AttackingCard.Legal.Visible = false;
                AttackingCard.SelectedTarget.Visible = false;
                AttackingCard.AttackIcon.Visible = false;
                foreach (var id in AttackingCard.ValidAttackTargets)
                {
                    CardsById[id].DefenseIcon.Visible = false;
                }

                if (AttackingCard.ValidAttackTargets.Contains(card.Id))
                {
                    card.SelectedTarget.Visible = true;
                    card.DefenseIcon.Visible = true;
                    IsActing = true;
                    EmitSignal(nameof(Attack), AttackingCard.Id, card.Id);
                }
                Attacking = false;
                AttackingCard = null;
                return;

            }
            switch (card.State)
            {
                case CardStates.CanBeDeployed:
                    if (User.State != States.Idle) { return; }
                    card.Legal.Visible = false;
                    IsActing = true;
                    EmitSignal(nameof(Deploy), card.Id);
                    break;
                case CardStates.CanBeSet:
                    if (User.State != States.Idle) { return; }
                    card.Legal.Visible = false;
                    IsActing = true;
                    EmitSignal(nameof(SetFaceDown), card.Id);
                    break;
                case CardStates.CanBeActivated:
                    if (User.State != States.Idle || User.State != States.Active) { return; }
                    if (card.Targets())
                    {
                        card.FlipFaceUp();
                        Targeting = true;
                        TargetingCard = card;
                        card.Legal.Visible = false;
                    }
                    else
                    {
                        card.Legal.Visible = false;
                        IsActing = true;
                        EmitSignal(nameof(Activate), card, new Array());
                    }
                    break;
                case CardStates.CanAttack:
                    if (User.State != States.Idle) { return; }
                    Attacking = true;
                    AttackingCard = card;
                    card.AttackIcon.Visible = true;
                    card.SelectedTarget.Visible = true;
                    break;
                case CardStates.Passive:
                    break;
                case CardStates.Activated:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
