using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game
{
    public class Catalog: IEnumerable<Card>
    {
        public IEnumerator<Card> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}