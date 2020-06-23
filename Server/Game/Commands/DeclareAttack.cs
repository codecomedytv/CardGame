using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Commands
{
    public class DeclareAttack : Command
    {
        public readonly ISource Source;
        public readonly Unit Attacker;
        public readonly Unit Defender;

        public DeclareAttack(Unit attacker, Unit defender)
        {
            Source = attacker;
            Attacker = attacker;
            Defender = defender;
        }

        public override void Execute()
        {
            
        }

        public override void Undo()
        {
            
        }
    }
}