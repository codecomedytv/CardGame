﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
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
            SetCode = SetCodes.AlphaNoviceArcher;
            Power = 1000;
            Skill = new OnSummonDestroy(this);
        }

        private class OnSummonDestroy : Automatic
        {
            public OnSummonDestroy(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Field);
                Triggers.Add(GameEvents.Deploy);
            }
            
            protected override bool _Trigger(Event gameEvent)
            {
                return gameEvent is Deploy deployed && deployed.Card == Card;
            }

            protected override async Task _Resolve()
            {
                AddTargets(Opponent.Field.Cast<Unit>().Where(unit => unit.Power <= 1000));
                if (ValidTargets.Count == 0)
                {
                    return;
                }
                RequestTarget();
                await ToSignal(this, nameof(Resumed));
                GD.Print("Resolving");
                GD.Print(Target != null);
                Destroy(Target);
            }
            
        }
    }
}