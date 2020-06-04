using System.Collections.Generic;

namespace CardGame.Server
{
    public class Support: Card
    {
        public Support() { }

        public override void SetCanBeSet()
        {
            CanBeSet = Zone == Controller.Hand && Controller.Support.Count < 7;
            if (CanBeSet) { Controller.DeclarePlay(new SetAsSettable(this)); }
        }
        

        public void SetAsActivatable(string gameEvent)
        {
            Skill.SetUp(gameEvent);
            if (!Skill.CanBeUsed)
            {
                return;
            }
            CanBeActivated = true;
            Controller.DeclarePlay(new Activate(this, new List<Card>()));
            
        }


    }
}