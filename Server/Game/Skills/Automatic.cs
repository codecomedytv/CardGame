using System;
using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game.Skills
{
    public class Automatic: Skill, IResolvable
    {
        public bool Triggered = false;
        
        public void Trigger(Command command)
        {
            if (!AreaOfEffects.Contains(Card.Zone))
            {
                return;
            }
            if (Triggers.Count > 0 && !Triggers.Contains(command.Identity))
            {
                return;
            }

            Triggered = true;
            _Trigger(command);
        }

        protected virtual void _Trigger(Command command)
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