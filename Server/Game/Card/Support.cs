using System.Collections.Generic;

namespace CardGame.Server
{
    public class Support: Card
    {

        public bool CanBeSet = false;
        public bool CanBeActivated = false;

        public Support()
        {
            SetAttributes();
            SetSkillCards();
            CreateSkills();
        }
        
        public override void OnControllerStateChanged(int state, string signal)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetAttributes()
        {
            throw new System.NotImplementedException();
        }

        protected override void _SetAsPlayable()
        {
            CanBeSet = true;
            Controller.DeclarePlay(new SetAsSettable(this));
        }

        public void SetAsSettable()
        {
            // Check for Anti-Set Tags
            CanBeSet = true;
            Controller.DeclarePlay(new SetAsSettable(this));
        }

        public void SetAsActivatable(string gameEvent)
        {
            foreach (var skill in Skills)
            {
                skill.SetUp(gameEvent);
                if (!skill.CanBeUsed)
                {
                    continue;
                }
                CanBeActivated = true;
                Controller.DeclarePlay(new Activate(this, new List<Card>()));
            }
        }
        
        
    }
}