using Godot.Collections;

namespace CardGame.Server.States
{
    public class Active: State
    {

        public override State OnActivation(Support card, int skillIndex, Array<int> targets)
        {
            Player.Link.Activate(Player, card, skillIndex, targets);
            return new Acting();
        }

        public override State OnPassPlay()
        {
            return new Passing();
        }
        
        public override string ToString()
        {
            return "Active";
        }
    }
}
