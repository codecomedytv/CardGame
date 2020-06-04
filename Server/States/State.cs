using Godot.Collections;

namespace CardGame.Server.States
{
    public class State: Godot.Object
    {
        protected Player Player;

        public virtual void OnEnter(Player player)
        {
            Player = player;
        }

        public virtual State OnDeploy(Unit unit)
        {
            Player.Disqualify();
            return new Disqualified();
        }

        public virtual State OnAttack()
        {
            Player.Disqualify();
            return new Disqualified();
        }

        public virtual State OnActivation(Support card, Array<int> targets)
        {
            Player.Disqualify();
            return new Disqualified();
        }

        public virtual State OnSetFaceDown(Support card)
        {
            Player.Disqualify();
            return new Disqualified();
        }

        public virtual State OnPassPlay()
        {
            Player.Disqualify();
            return new Disqualified();
        }

        public virtual State OnEndTurn()
        {
            Player.Disqualify();
            return new Disqualified();
        }

        public override string ToString()
        {
            return "State";
        }
    }
}