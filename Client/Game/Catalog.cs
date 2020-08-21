using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game
{
    public class Catalog: IEnumerable<Card>
    {
        private readonly List<Card> Cards = new List<Card>();
        
        
        public IEnumerator<Card> GetEnumerator()
        {
            return Cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}