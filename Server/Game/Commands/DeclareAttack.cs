using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server.Game.Commands
{
    public class DeclareAttack : Command
    {
        public readonly ISource Source;
        public readonly Unit Attacker;
        public readonly Unit Defender;
        public readonly IResolvable Attack;

        public DeclareAttack(Unit attacker, Unit defender, IResolvable attack)
        {
            GameEvent = GameEvents.DeclareAttack;
            Source = attacker;
            Attacker = attacker;
            Defender = defender;
            Attack = attack;
        }

        public override void Execute()
        {
            
        }

        public override void Undo()
        {
            
        }
    }
}