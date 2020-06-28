#nullable enable
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server.Game.Events
{
    public class Activate: Event
    {
        public readonly ISource Source;
        public readonly Support Card;
        public readonly Manual Skill;
        public readonly Card? Target;

        public Activate(ISource source, Support card, Manual skill, Card? target = null)
        {
            Identity = GameEvents.Activate;
            Source = source;
            Card = card;
            Skill = skill;
            Target = target;
        }

    }
}
