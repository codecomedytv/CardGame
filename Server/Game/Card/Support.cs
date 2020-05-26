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
            Controller.DeclarePlay("BlankEvent");
        }

        public void SetAsSettable()
        {
            // Check for Anti-Set Tags
            CanBeSet = true;
            Controller.DeclarePlay("BlankEvent");
        }

        public void SetAsActivatable(string gameEvent)
        {
            foreach (var skill in Skills)
            {
                skill.SetUp(gameEvent);
                if (skill.CanBeUsed)
                {
                    CanBeActivated = true;
                    Controller.DeclarePlay("BlankEvent");
                }
            }
        }
        
        
    }
}