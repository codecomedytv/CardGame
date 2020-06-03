namespace CardGame.Server.States
{
    public class Idle: State
    {
        
        public override State OnDeploy()
        {
            return new Acting();
        }

        public override State OnAttack()
        {
            return new Acting();
        }

        public override State OnActivation()
        {
            return new Acting();
        }

        public override State OnPassPlay()
        {
            return new Disqualified();
        }

        public override State OnEndTurn()
        {
            return new Idle();
        }
    }
}