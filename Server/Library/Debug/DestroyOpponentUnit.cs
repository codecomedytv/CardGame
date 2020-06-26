using System.Diagnostics.Tracing;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server
{
    public class DestroyOpponentUnit : Support
    {
        public DestroyOpponentUnit(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.DestroyOpponentUnit";
            SetCode = SetCodes.DebugDestroyOpponentUnit;
            Skill = new DestroyUnit(this);
        }

        private class DestroyUnit : Skill
        {
            public DestroyUnit(Card card)
            {
                Card = card;
            }
            protected override void _SetUp()
            {
                var units = Opponent.Field;
                SetTargets(units.ToList());
                CanBeUsed = units.Count > 0;
            }

            protected override void _Resolve()
            {
                Destroy(Target);
            }
        }
    }
}