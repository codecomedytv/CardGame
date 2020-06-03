namespace CardGame.Server.States
{
    public class Active: State
    {

        public override State OnActivation()
        {
            throw new System.NotImplementedException();
        }

        public override State OnPassPlay()
        {
            return new Passing();
        }
    }
}