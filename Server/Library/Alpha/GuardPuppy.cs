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
            AddSkill(new BattleImmunity());
        }

        private class BattleImmunity : Skill
        {
            private readonly Tag Tag = new Tag(TagIds.CannotBeAttacked);
            public BattleImmunity()
            {
                Type = Types.Constant;
            }

            protected override void _Resolve()
            {
                if (!Controller.Field.Contains(Card))
                {
                    foreach (var card in Controller.Field)
                    {
                        card.Tags.Remove(Tag);
                        return;
                    }
                }
                foreach (var card in Controller.Field)
                {
                    if (card == Card)
                    {
                        continue;
                    }

                    // Not to be confused with HasTag which only cares about the ID
                    if (card.Tags.Contains(Tag))
                    {
                        continue;
                    }

                    card.Tags.Add(Tag);
                }
            }
        }
    }
}