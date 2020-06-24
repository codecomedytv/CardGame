using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skill;

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
                Mill(Controller.Deck.Top);
            }
        }
    }
}