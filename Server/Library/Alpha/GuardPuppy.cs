using CardGame.Server.Game;
using CardGame.Server.Game.Cards;

namespace CardGame.Server
{
    public class GuardPuppy: Unit
    {
        public GuardPuppy()
        {
            Title = "Guard Puppy";
            SetCode = SetCodes.Alpha_GuardPuppy;
            Attack = 500;
            Defense = 500;
            var decorator = new Decorator(Tag.CannotBeDestroyedByBattle);
            decorator.AddTagTo(this);
            AddSkill(new BattleImmunity());
        }

        private class BattleImmunity : Skill
        {
            public BattleImmunity()
            {
                GameEvent = "deploy";
                Type = Types.Constant;
            }

            protected override void _Resolve()
            {
                AddTagToGroup(Controller.Field, Tag.CannotBeAttacked, nameof(Card.Exit), nameof(Card.Exit), false);
            }
        }
    }
}