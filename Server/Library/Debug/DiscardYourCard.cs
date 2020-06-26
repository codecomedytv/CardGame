using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class DiscardYourCard : Support
    {
        public DiscardYourCard(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.DiscardYourCard";
            SetCode = SetCodes.DebugDiscardYourCard;
            Skill = new DiscardCard(this);
        }

        private class DiscardCard : Skill
        {
            public DiscardCard(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Support);

            }
            protected override void _SetUp()
            {
                AddTargets(Controller.Hand);
                CanBeUsed = ValidTargets.Count > 0;
            }

            protected override void _Resolve()
            {
                Discard(Target);
            }
        }
    }
}