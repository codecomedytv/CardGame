using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server.Game.Cards
{
    public class Support: Card
    {
        protected Support() { }

        public override void SetCanBeSet()
        {
            CanBeSet = Zone == Controller.Hand && Controller.Support.Count < 7;
        }

        public override void SetCanBeActivated()
        {
            CanBeActivated = Skill is Manual skill && skill.CanBeUsed && IsReady;
        }

    }
}