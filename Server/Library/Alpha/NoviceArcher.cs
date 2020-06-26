﻿using System.Collections.Generic;
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
                Triggers.Add(GameEvents.Deploy);
                Type = Types.Auto;
            }

            protected override void _SetUp()
            {
                var units = (from Unit u in Opponent.Field where u.Attack < 1000 select u).ToList();
                CanBeUsed = units.Count > 0;
            }

            protected override void _Activate()
            {
                var units = (from Unit u in Opponent.Field where u.Attack < 1000 select u).Cast<Card>().ToList();
                SetTargets(units);
                RequestTarget();
            }

            protected override void _Resolve()
            {
                Destroy(Target);
            }
        }
    }
}