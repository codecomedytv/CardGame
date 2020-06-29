#nullable enable
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game.Skills
{
    public class Manual: Skill, IResolvable
    {
        public bool CanBeUsed;
        public int PositionInLink { get; private set; }

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
            if(CanBeUsed && Card is Support)
            {
                Card.State = Card.States.CanBeActivated;
            }
        }

        protected virtual void _SetUp()
        {
            CanBeUsed = true;
        }
        
        public void Activate(Card? target, int positionInLink)
        {
            PositionInLink = positionInLink;
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
            PositionInLink = 0;
        }
		
        protected virtual void _Resolve()
        {
        }
    }
}