using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;

namespace CardGame.Server
{
    public class PlayerCannotTakeDamage : Unit
    {
        public PlayerCannotTakeDamage(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.PlayerCannotTakeDamage";
            Attack = 1000;
            Defense = 1000;
            SetCode = SetCodes.DebugPlayerCannotTakeDamage;
            Skill = new PlayerCannotTakeBattleDamage(this);
        }

        private class PlayerCannotTakeBattleDamage: Skill
        {
            private readonly Tag Tag = new Tag(TagIds.CannotTakeBattleDamage);
            public PlayerCannotTakeBattleDamage(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Field);
                Type = Types.Constant;
            }

            protected override void _Resolve()
            {
                Tag.UnTagAll();
                if (Controller.Field.Contains(Card))
                {
                    Tag.Add(Controller);
                }
            }
        }
    }
}

