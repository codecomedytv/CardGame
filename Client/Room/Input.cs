using CardGame.Client.Library.Cards;
using Godot;
using Godot.Collections;

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
            card.Connect(nameof(Card.MouseEnteredCard), this, nameof(OnMouseEnterCard), new Array {card});
            card.Connect(nameof(Card.MouseExitedCard), this, nameof(OnMouseExitCard), new Array {card});
            card.Connect(nameof(Card.DoubleClicked), this, nameof(OnCardDoubleClicked), new Array {card});
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
                    CardCatalog.Fetch(id).ValidTarget.Visible = true;
                }
            }

            else if (card.Attacks() && User.State == States.Idle)
            {
                card.AttackIcon.Visible = true;
                foreach (var id in card.ValidAttackTargets)
                {
                    CardCatalog.Fetch(id).DefenseIcon.Visible = true;
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
                CardCatalog.Fetch(id).ValidTarget.Visible = false;
            }

            foreach (var id in card.ValidAttackTargets)
            {
                CardCatalog.Fetch(id).DefenseIcon.Visible = false;
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
                    CardCatalog.Fetch(id).ValidTarget.Visible = false;
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
                AttackingCard.Legal.Visible = false;
                foreach (var id in AttackingCard.ValidAttackTargets)
                {
                    CardCatalog.Fetch(id).DefenseIcon.Visible = false;
                }

                if (AttackingCard.ValidAttackTargets.Contains(card.Id))
                {
                    AttackingCard.SelectedTarget.Visible = true;
                    AttackingCard.AttackIcon.Visible = true;
                    card.SelectedTarget.Visible = true;
                    card.DefenseIcon.Visible = true;
                    User.State = States.Processing;
                    EmitSignal(nameof(Attack), AttackingCard.Id, card.Id);
                }
                else
                {
                    AttackingCard.AttackIcon.Visible = false;
                    AttackingCard.SelectedTarget.Visible = false;
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
                    User.State = States.Processing;
                    EmitSignal(nameof(Deploy), card.Id);
                    break;
                case CardStates.CanBeSet:
                    if (User.State != States.Idle) { return; }
                    card.Legal.Visible = false;
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
                        card.Legal.Visible = false;
                    }
                    else
                    {
                        card.Legal.Visible = false;
                        User.State = States.Processing;
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
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}