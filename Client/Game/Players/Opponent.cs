using System.Collections.Generic;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Players
{
    public class Opponent: IPlayer
    {
        private readonly IPlayerModel Model;
        private readonly IPlayerView View;

        public Opponent(IPlayerView view)
        {
            Model = new PlayerModel();
            View = view;
        }

        public void Connect(Declaration addCommand)
        {
            View.Connect(addCommand);
        }

        public void LoadDeck(IEnumerable<Card> deck)
        {
            foreach (var card in deck)
            {
                View.AddCardToDeck(card);
            }
        }

        public void Draw(Card card)
        {
            Model.Draw(card);
            View.Draw(card);
        }

        public void Discard(Card card)
        {
            throw new System.NotImplementedException();
        }

        public void Deploy(Card card)
        {
            throw new System.NotImplementedException();
        }

        public void Set(Card card)
        {
            throw new System.NotImplementedException();
        }
    }
}