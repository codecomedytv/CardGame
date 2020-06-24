using CardGame.Server.Game.Commands;
using Godot;

namespace CardGame.Server.Game.Cards
{
    public class Support: Card
    {
        public Support() { }

        public override void SetCanBeSet()
        {
            CanBeSet = Zone == Controller.Hand && Controller.Support.Count < 7;
        }

        public override void SetCanBeActivated()
        {
            CanBeActivated = Skill.CanBeUsed && IsReady;
        }

    }
}