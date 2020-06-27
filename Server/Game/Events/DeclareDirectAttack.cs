using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
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
        
    }
}