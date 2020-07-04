using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server.Game.Cards
{
    public class Support: Card
    {
        protected Support() { }

        public override void SetCanBeSet()
        {
            State = Zone == Controller.Hand && Controller.Support.Count < 7 ? States.CanBeSet : States.Passive;
        }

        public override void SetCanBeActivated()
        {
            State = Skill is Manual skill && skill.CanBeUsed && IsReady ? States.CanBeActivated : States.Passive;
        }

    }
}