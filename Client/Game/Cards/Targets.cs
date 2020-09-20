using System.Collections.Generic;

namespace CardGame.Client.Game.Cards
{
    public class Targets
    {
        private IList<Card> Cards = new List<Card>();

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
    }
}