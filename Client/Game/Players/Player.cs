using System.Collections.Generic;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Players
{
    public class Player: IPlayer
    {
        public void ConnectCommands(CommandQueue commandQueue)
        {
            throw new System.NotImplementedException();
        }

        public void LoadDeck(IEnumerable<Card> deck)
        {
            
        }

        public void Draw(Card card)
        {
            throw new System.NotImplementedException();
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