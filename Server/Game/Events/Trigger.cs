using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server.Game.Events
{
    public class Trigger: Event
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;
        public readonly Automatic Skill;
        public readonly int PositionInLink;
        
        public Trigger(ISource source, Card card, Automatic skill)
        {
            Identity = GameEvents.Activate;
            Player = card.Controller;
            Source = source;
            Card = card;
            Skill = skill;
            PositionInLink = skill.PositionInLink;
        }

        public override void SendMessage(Message message)
        {
            message(Player.Id, Commands.Trigger, Card.Id, PositionInLink);
            message(Player.Opponent.Id, Commands.Trigger, Card.Id, PositionInLink);
        }
    }
}