using CardGame.Server.Room.Cards;
using CardGame.Server.Room.Commands;

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
                var toMill = Controller.Deck[Controller.Deck.Count - 1];
                Controller.DeclarePlay(new Move(Card, toMill, Controller.Graveyard));
            }
        }
    }
}