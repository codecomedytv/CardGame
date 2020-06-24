using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Commands
{
    public class DeclareDirectAttack : Command
    {
        public readonly GameEvents Identity = GameEvents.DeclareDirectAttack;
        public readonly ISource Source;
        public readonly Unit Attacker;

        public DeclareDirectAttack(Unit attacker)
        {
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