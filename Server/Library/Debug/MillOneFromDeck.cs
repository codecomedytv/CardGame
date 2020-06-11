using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;

namespace CardGame.Server
{
    public class MillOneFromDeck : Support
    {
        public MillOneFromDeck()
        {
            Title = "Debug.MillOneFromCard";
            SetCode = SetCodes.MillOneFromDeck;
            AddSkill(new MillCard());
        }

        private class  MillCard: Skill
        {
            protected override void _Resolve()
            {
                Controller.DeclarePlay(new Mill(Card, Controller, Controller.Deck[Controller.Deck.Count-1]));
            }
        }
    }
}