#nullable enable
using CardGame.Client.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Zones;

namespace CardGame.Server.Game.Events
{
    public class Activate: Event
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Support Card;
        public readonly Manual Skill;
        public readonly Card? Target;
        public readonly int PositionInLink;

        
        public Activate(ISource source, Support card, Manual skill, Card? target = null)
        {
            Player = (Player) source;
            Identity = GameEvents.Activate;
            Source = source;
            Card = card;
            Skill = skill;
            Target = target;
            PositionInLink = skill.PositionInLink;
        }

        private int TargetId() => Target?.Id ?? 0;

        public override void SendMessage(Message message)
        {
            var knownTarget = 0;
            message(Player.Id, Commands.Activate, Card.Id, !IsOpponent, knownTarget);
            message(Player.Opponent.Id, Commands.RevealCard, Card.Id, Card.SetCode, ZoneIds.Support);
            message(Player.Opponent.Id, Commands.Activate, Card.Id, IsOpponent, TargetId());
        }
    }
}
