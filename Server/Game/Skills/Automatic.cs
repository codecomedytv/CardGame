using System;
using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game.Skills
{
    public class Automatic: Skill, IResolvable
    {
        public bool Triggered = false;
        
        public void Trigger(Event Event)
        {
            if (!AreaOfEffects.Contains(Card.Zone))
            {
                return;
            }
            if (Triggers.Count > 0 && !Triggers.Contains(Event.Identity))
            {
                return;
            }

            Triggered = true;
            _Trigger(Event);
        }

        protected virtual void _Trigger(Event Event)
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