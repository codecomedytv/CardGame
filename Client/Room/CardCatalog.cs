using System.Linq;
using CardGame.Client.Library.Cards;
using CardGame.Client.Player;
using Godot;
using Godot.Collections;

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
        
        private readonly System.Collections.Generic.Dictionary<int, Card> CardsById = new System.Collections.Generic.Dictionary<int, Card>();
        public Controller User;
        
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
            //card.Connect(nameof(Card.Clicked), this, nameof(OnCardClicked), new Array {card});
            card.Connect(nameof(Card.DoubleClicked), this, nameof(this.OnCardDoubleClicked), new Array {card});
            return card;
        }

        private void OnMouseEnterCard(Card card)
        {
            EmitSignal(nameof(MouseEnteredCard), card);
            var playingState = User.Model.State == States.Idle || User.Model.State == States.Active;
            if (card.State == CardStates.Passive || card.State == CardStates.Activated || !playingState)
            {
                return;
            }
            card.Legal.Visible = true;
        }

        private void OnMouseExitCard(Card card)
        {
            card.Legal.Visible = false;
        }
        
        private void OnCardDoubleClicked(Card card)
        {
            switch (card.State)
            {
                case CardStates.CanBeDeployed:
                    card.Legal.Visible = false;
                    EmitSignal(nameof(Deploy), card.Id);
                    break;
                case CardStates.CanBeSet:
                    card.Legal.Visible = false;
                    EmitSignal(nameof(SetFaceDown), card.Id);
                    break;
            }
        }
    }
}
