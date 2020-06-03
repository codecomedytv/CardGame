namespace CardGame.Server.States
{
    public class State: Godot.Object
    {
        protected Player player;

        public virtual State OnDeploy()
        {
            return new Disqualified();
        }

        public virtual State OnAttack()
        {
            return new Disqualified();
        }

        public virtual State OnActivation()
        {
            return new Disqualified();
        }

        public virtual State OnPassPlay()
        {
            return new Disqualified();
        }

        public virtual State OnEndTurn()
        {
            return new Disqualified();
        }
    }
}