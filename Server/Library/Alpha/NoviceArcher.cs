using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server
{
    public class NoviceArcher : Unit
    {
        public NoviceArcher(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Novice Archer";
            SetCode = SetCodes.Alpha_NoviceArcher;
            Attack = 1000;
            Defense = 1000;
            Skill = new OnSummonDestroy(this);
        }

        private class OnSummonDestroy : Skill
        {
            public OnSummonDestroy(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Field);
                Triggers.Add(GameEvents.Deploy);
                Type = Types.Auto;
            }

            protected override void _SetUp()
            {
                AddTargets(Opponent.Field.Cast<Unit>().Where(unit => unit.Attack < 1000));
                CanBeUsed = ValidTargets.Count > 0;
            }

            protected override void _Activate()
            {
                AddTargets(Opponent.Field.Cast<Unit>().Where(unit => unit.Attack < 1000));
                RequestTarget();
            }

            protected override void _Resolve()
            {
                Destroy(Target);
            }
        }
    }
}