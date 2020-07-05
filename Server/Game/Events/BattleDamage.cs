namespace CardGame.Server.Game.Events
{
    public class BattleDamage: Event
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly int Dealt;

        public BattleDamage(ISource source, Player player, int damage)
        {
            Source = source;
            Player = player;
            Dealt = damage;
        }

        public override void SendMessage(Message message)
        {
            message(Player.Id, "LoseLife", Dealt, false);
            message(Player.Opponent.Id, "LoseLife", Dealt, true);
        }
    }
}