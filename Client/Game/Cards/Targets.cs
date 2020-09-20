using System.Collections;
using System.Collections.Generic;

namespace CardGame.Client.Game.Cards
{
    public class Targets: IEnumerable<Card>
    {
        private IList<Card> Cards = new List<Card>();

        public int Count => Cards.Count;
        public void Update(IList<Card> newTargets)
        {
            Cards = newTargets;
        }

        public void Highlight()
        {
            foreach (var target in Cards)
            {
                target.Target();
            }
        }

        public void StopHighlighting()
        {
            foreach (var target in Cards)
            {
                target.StopTargeting();
            }
        }

        public IEnumerator<Card> GetEnumerator() => Cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}