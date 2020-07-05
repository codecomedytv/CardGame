using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class BattleUnit: Event
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Attacker;
        public readonly Card Defender;

        public BattleUnit(ISource source, Card attacker, Card defender)
        {
            Source = source;
            Player = (Player) source;
            Attacker = attacker;
            Defender = defender;
        }

        public override void SendMessage(Message message)
        {
            message(Player.Id, "BattleUnit", Attacker.Id, Defender.Id, false);
            message(Player.Opponent.Id, "BattleUnit", Attacker.Id, Defender.Id, true);
        }
    }
}