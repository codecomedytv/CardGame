using System;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game.Skills
{
    public class Automatic: Skill, IResolvable
    {
        public bool Triggered = false;
        public int PositionInLink { get; private set; }
        
        public void Trigger(Event gameEvent, int positionInLinkIfTriggered)
        {
            if (!AreaOfEffects.Contains(Card.Zone))
            {
                return;
            }
            if (Triggers.Count > 0 && !Triggers.Contains(gameEvent.Identity))
            {
                return;
            }
            Triggered = true;
            _Trigger(gameEvent);
            PositionInLink = positionInLinkIfTriggered;
            History.Add(new Trigger(Card, Card, this));
        }

        protected virtual void _Trigger(Event gameEvent)
        {
            throw new NotImplementedException();
        }
        
        public void Resolve()
        {
            _Resolve();
            EmitSignal(nameof(Resolved));
            PositionInLink = 0;
        }
		
        protected virtual void _Resolve()
        {
        }
    }
}