using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class ShowAttack: Message
    {
        public ShowAttack(Unit attacker, Unit defender)
        {
            Player[Command] = (int) GameEvents.AttackDeclared;
            Player["attackerId"] = attacker.Id;
            Player["defenderId"] = defender.Id;
        }
    }
}
