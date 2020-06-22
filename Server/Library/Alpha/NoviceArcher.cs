using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CardGame.Server.Room.Cards;
using CardGame.Server.Room.Commands;
using Godot;

namespace CardGame.Server
{
    public class NoviceArcher : Unit
    {
        public NoviceArcher()
        {
            Title = "Novice Archer";
            SetCode = SetCodes.Alpha_NoviceArcher;
            Attack = 1000;
            Defense = 1000;
            AddSkill(new OnSummonDestroy());
        }

        private class OnSummonDestroy : Skill
        {
            public OnSummonDestroy()
            {
                GameEvent = "deploy";
                Type = Types.Auto;
            }

            public override void _SetUp()
            {
                var units = (from Unit u in Opponent.Field where u.Attack < 1000 select u).ToList();
                CanBeUsed = units.Count > 0;
            }

            protected override void _Activate()
            {
                var units = (from Unit u in Opponent.Field where u.Attack < 1000 select u).Cast<Card>().ToList();
                SetTargets(units);
                AutoTarget();
            }

            protected override void _Resolve()
            {
                Controller.DeclarePlay(new Move(Card, Target.Owner, Target, Target.Owner.Graveyard));
            }
        }
    }
}