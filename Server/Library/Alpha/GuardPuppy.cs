using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;
using Godot;

namespace CardGame.Server
{
    public class GuardPuppy: Unit
    {
        public GuardPuppy(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Guard Puppy";
            SetCode = SetCodes.Alpha_GuardPuppy;
            Attack = 500;
            Defense = 500;
            Skill = new BattleImmunity(this);
        }

        private class BattleImmunity : Constant
        {
            private readonly Tag Tag = new Tag(TagIds.CannotBeAttacked);
            public BattleImmunity(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Field);
                AreaOfEffects.Add(Controller.Graveyard);
                Type = Types.Constant;
            }

            protected override void _Apply()
            {
                // We refresh tags on each update cycle
                Tag.UnTagAll();
                if (!Controller.Field.Contains(Card))
                {
                    return;
                }
                foreach (var card in Controller.Field)
                {
                    if (card == Card)
                    {
                        continue;
                    }

                    Tag.Add(card);
                }
            }
        }
    }
}