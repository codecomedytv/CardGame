using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game.Cards
{
    public class Support: Card
    {
        public Support() { }

        public override void SetCanBeSet()
        {
           if (Zone == Controller.Hand && Controller.Support.Count < 7) 
           { Controller.Match.History.Add(new ModifyCard(Controller, this, nameof(CanBeSet), true)); }
           else
           {
               // This exists to explicitly declare it an illegal player for a disqualification but I'm not sure if
               // this is a problematic work-around or that the other cards don't exist properly yet.
               Controller.Match.History.Add(new ModifyCard(Controller, this, nameof(CanBeSet), false));
           }
        }

        public override void SetCanBeActivated()
        {
            if (Skill.CanBeUsed) {Controller.Match.History.Add(new ModifyCard(Controller, this, nameof(CanBeActivated), true));}
        }

    }
}