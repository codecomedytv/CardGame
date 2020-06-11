using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class AttackDirectly: Message
    {
        public AttackDirectly(Unit attacker)
        {
            Player[Command] = (int) GameEvents.AttackedDirectly;
            Player[Id] = attacker.Id;
            Opponent[Command] = (int) GameEvents.OpponentAttackedDirectly;
            Opponent[Id] = attacker.Id;
        }
    }
}
