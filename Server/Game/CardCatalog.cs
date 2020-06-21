using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game
{
    public class CardCatalog
    {
        private int NextCardId = 0;
        private readonly Dictionary<int, Card> CardsById = new Dictionary<int, Card>();
        
        public void RegisterCard(Card card)
        {
            card.Id = NextCardId;
            CardsById[card.Id] = card;
            NextCardId += 1;
        }

        public Card GetCard(int id) => CardsById[id];
    }
}