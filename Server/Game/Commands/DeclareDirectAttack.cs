using System.Diagnostics.Contracts;
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Commands
{
    public class DeclareDirectAttack : Event
    {
        public readonly ISource Source;
        public readonly Unit Attacker;
        public readonly Unit.DirectAttack DirectAttack;

        public DeclareDirectAttack(Unit attacker, Unit.DirectAttack directAttack)
        {
            GameEvent = GameEvents.DeclareDirectAttack;
            DirectAttack = directAttack;
            Source = attacker;
            Attacker = attacker;
        }

        public override void Execute()
        {
        }

        public override void Undo()
        {
        }
    }
}