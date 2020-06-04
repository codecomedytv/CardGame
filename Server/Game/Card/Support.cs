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