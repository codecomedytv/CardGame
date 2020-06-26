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
            AddSkill(new DiscardCard());
        }

        private class DiscardCard : Skill
        {
            protected override void _SetUp()
            {
                SetTargets(Controller.Hand.ToList());
                CanBeUsed = Controller.Hand.Count > 0;
            }

            protected override void _Resolve()
            {
                Discard(Target);
            }
        }
    }
}