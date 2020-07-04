using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server.Game.Cards
{
    public class Support: Card
    {
        protected Support() { }

        public override void SetState()
        {
            State = States.Passive;
            if (Zone == Controller.Hand && Controller.Support.Count < 7)
            {
                State = States.CanBeSet;
            }

            if (Skill is Manual skill && skill.CanBeUsed && IsReady)
            {
                State = States.CanBeActivated;
            }
        }
        

    }
}