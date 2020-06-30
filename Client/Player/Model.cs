using System.Collections.Generic;
using CardGame.Client.Library.Cards;

namespace CardGame.Client.Player
{
    public class Model
    {
        public States State;
        public int Health = 8000;
        public int DeckCount = 40;
        public IList<Card> Units = new List<Card>();
        public IList<Card> Support = new List<Card>();
        public IList<Card> Hand = new List<Card>();
        public IList<Card> Graveyard = new List<Card>();
        
    }
}