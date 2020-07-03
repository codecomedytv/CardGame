#nullable enable
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Network;
using CardGame.Server.Game.Skills;

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

        public override void SendMessage(Message message)
        {
            message(Player.Id, "Activation", Card.Id, PositionInLink);
            message(Player.Opponent.Id, "Activation", Card.Id, Card.SetCode, PositionInLink);
        }
    }
}
