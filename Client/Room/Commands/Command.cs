namespace CardGame.Client.Room.Commands
{
    public abstract class Command
    {
        protected Command()
        {
            
        }

        public abstract void Execute();
    }
}