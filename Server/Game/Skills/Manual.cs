using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game.Skills
{
    public class Manual: Skill
    {
        public void SetUp(Command gameEvent)
        {
            if (!AreaOfEffects.Contains(Card.Zone))
            {
                return;
            }
            
            // Doesn't seem right?
            if (Triggers.Count > 0 && !Triggers.Contains(gameEvent.Identity))
            {
                return;
            }

            _SetUp();
            if(CanBeUsed && Card is Support support)
            {
                support.CanBeActivated = true;
            }
        }

        protected virtual void _SetUp()
        {
            CanBeUsed = true;
        }
        
        public void Activate()
        {
            Card.Activated = true;
            CanBeUsed = false;
            _Activate();
        }
		
        protected virtual void _Activate()
        {
        }
    }
}