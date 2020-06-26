#nullable enable
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server.Game.Commands
{
    public class Activate: Command
    {
        public readonly ISource Source;
        public readonly Support Card;
        public readonly Manual Skill;
        public readonly Card? Target;

        public Activate(ISource source, Support card, Manual skill, Card? target = null)
        {
            GameEvent = GameEvents.Activate;
            Source = source;
            Card = card;
            Skill = skill;
            Target = target;
        }
        public override void Execute()
        {
            // GameEvent
        }

        public override void Undo()
        {
            // GameEvent
        }
    }
}
