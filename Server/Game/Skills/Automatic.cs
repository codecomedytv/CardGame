using System;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game.Skills
{
    public class Automatic: Skill, IResolvable
    {
        public bool Triggered = false;
        
        public void Trigger(Event gameEvent)
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
        }

        protected virtual void _Trigger(Event gameEvent)
        {
            throw new NotImplementedException();
        }
        
        public void Resolve()
        {
            _Resolve();
            EmitSignal(nameof(Resolved));
        }
		
        protected virtual void _Resolve()
        {
        }
    }
}