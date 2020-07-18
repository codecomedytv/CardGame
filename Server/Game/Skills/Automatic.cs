using System;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game.Skills
{
    public class Automatic: Skill, IResolvable
    {
        public bool Triggered { get; private set; } = false;
        public int PositionInLink { get; private set; }
        
        public void Trigger(Event gameEvent, int positionInLinkIfTriggered)
        {
            ValidTargets.Clear();
            if (!AreaOfEffects.Contains(Card.Zone))
            {
                return;
            }
            if (Triggers.Count > 0 && !Triggers.Contains(gameEvent.Identity))
            {
                return;
            }
            Triggered = _Trigger(gameEvent);
            PositionInLink = positionInLinkIfTriggered;
            History.Add(new Trigger(Card, Card, this));
        }

        protected virtual bool _Trigger(Event gameEvent)
        {
            return true;
        }
        
        public void Resolve()
        {
            _Resolve();
            History.Add(new ResolveCard(Card, Target));
            EmitSignal(nameof(Resolved));
            PositionInLink = 0;
        }
		
        protected virtual void _Resolve()
        {
        }
    }
}