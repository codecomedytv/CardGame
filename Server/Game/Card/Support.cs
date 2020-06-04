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

        public override void SetCanBeActivated()
        {
            CanBeActivated = Skill.CanBeUsed;
            if (CanBeActivated) {Controller.DeclarePlay(new Activate(this, new List<Card>()));}
        }

    }
}