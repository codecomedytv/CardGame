using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class AttackUnit: Message
    {
        public AttackUnit(Unit attacker, Unit defender)
        {
            Player[Command] = (int) GameEvents.AttackedUnit;
            Player["attackerId"] = attacker.Id;
            Player["defenderId"] = defender.Id;
            Opponent[Command] = (int) GameEvents.OpponentAttackedUnit;
            Opponent["attackerId"] = attacker.Id;
            Opponent["defenderId"] = defender.Id;
        }
    }
}