using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            Attack = 1000;
            Defense = 1000;
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

            protected override async void _Resolve()
            {
                AddTargets(Opponent.Field.Cast<Unit>().Where(unit => unit.Attack < 1000));
                if (ValidTargets.Count == 0)
                {
                    return;
                }
                RequestTarget();
                var results = await ToSignal(Controller, nameof(Player.TargetSelected));
                Target = results[0] as Card;
                Destroy(Target);
            }
        }
    }
}