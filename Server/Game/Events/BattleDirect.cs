using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class BattleDirect: Event
    {
        public readonly ISource Source;
        public readonly Player Attacking;
        public readonly Card Attacker;

        public BattleDirect(ISource source, Player attacking, Card attacker)
        {
            Source = source;
            Attacking = attacking;
            Attacker = attacker;
        }

        public override void SendMessage(Message message)
        {
            message(Attacking.Id, CommandId.DirectAttack, Attacker.Id, true);
            message(Attacking.Opponent.Id, CommandId.DirectAttack, Attacker.Id, false);
        }
    }
}
