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
            Identity = GameEvents.DeclareDirectAttack;
            DirectAttack = directAttack;
            Source = attacker;
            Attacker = attacker;
        }

        public override void SendMessage(Message message)
        {
            message(Attacker.Opponent.Id, CommandId.OpponentAttackDirectly, Attacker.Id);
        }
    }
}