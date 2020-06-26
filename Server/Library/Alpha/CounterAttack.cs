﻿using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class CounterAttack: Support
    {
        public CounterAttack()
        {
            Title = "CounterAttack";
            SetCode = SetCodes.Alpha_CounterAttack;
            AddSkill(new DestroyAttacker());
        }

        private class DestroyAttacker: Skill
        {
            public DestroyAttacker()
            {
                Triggers.Add(GameEvents.DeclareAttack);
            }

            protected override void _Resolve()
            {
                Destroy(Match.Attacking);
            }
        }
    }
}