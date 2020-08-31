using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server.Game.Events
{
    public class DeclareAttack : Event
    {
        public readonly ISource Source;
        public readonly Unit Attacker;
        public readonly Unit Defender;
        public readonly IResolvable Attack;

        public DeclareAttack(Unit attacker, Unit defender, IResolvable attack)
        {
            Identity = GameEvents.DeclareAttack;
            Source = attacker;
            Attacker = attacker;
            Defender = defender;
            Attack = attack;
        }
        
        public override void SendMessage(Message message)
        {
            message(Attacker.Opponent.Id, Commands.OpponentAttackUnit, Attacker.Id, Defender.Id);
        }

        
    }
}