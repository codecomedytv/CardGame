﻿using System.Collections.Generic;
using System.Linq;

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

        public class OnSummonDestroy : Skill
        {
            public OnSummonDestroy()
            {
                Type = Types.Auto;
                GameEvent = "deploy";
            }

            public override void _SetUp()
            {
                // IEnumerable<Unit> units = (IEnumerable<Unit>)Opponent.Field.Where(u => u.Attack > 1000).ToList();
                var units = new List<Unit>();
                foreach (var unit in Opponent.Field)
                {
                    Unit u = (Unit) unit;
                    if (u.Attack > 1000)
                    {
                        units.Add(u);
                    }
                }
                CanBeUsed = units.Count > 0;
            }

            public override void _Activate()
            {
                // SetTargets(Opponent.Field.Where(u => u.Attack > 1000));
                var units = new List<Card>();
                foreach (var unit in Opponent.Field)
                {
                    Unit u = (Unit) unit;
                    if (u.Attack > 1000)
                    {
                        units.Add(u);
                    }
                }
                SetTargets(units);
                AutoTarget();
            }

            public override void _Resolve()
            {
                Controller.DestroyUnit((Unit)GameState.Target);
            }
        }
    }
}