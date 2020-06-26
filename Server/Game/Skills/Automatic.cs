using System;
using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game.Skills
{
    public class Automatic: Skill
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
    }
}