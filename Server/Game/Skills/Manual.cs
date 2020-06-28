#nullable enable
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;

namespace CardGame.Server.Game.Skills
{
    public class Manual: Skill, IResolvable
    {
        public bool CanBeUsed;
        public void SetUp(Event gameEvent)
        {
            if (!AreaOfEffects.Contains(Card.Zone))
            {
                return;
            }
            
            if (Triggers.Count > 0 && !Triggers.Contains(gameEvent.Identity))
            {
                return;
            }

            _SetUp();
            if(CanBeUsed && Card is Support support)
            {
                support.State = Card.States.CanBeActivated;
            }
        }

        protected virtual void _SetUp()
        {
            CanBeUsed = true;
        }
        
        public void Activate(Card? target)
        {
            Target = target;
            Card.State = Card.States.CanBeActivated;
            CanBeUsed = false;
            History.Add(new Activate(Controller, (Support) Card, this, target));
        }
        
        public void Resolve()
        {
            _Resolve();
            Card.State = Card.States.Passive;
            Controller.Support.Remove(Card);
            Owner.Graveyard.Add(Card);
            EmitSignal(nameof(Resolved));
        }
		
        protected virtual void _Resolve()
        {
        }
    }
}