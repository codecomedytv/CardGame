namespace CardGame.Server.Game.Commands
{
    public class EndTurn: Command
    {
        public readonly ISource Source;

        public EndTurn(ISource source)
        {
            Source = source;
        }

        public override void Execute()
        {
            
        }

        public override void Undo()
        {
            
        }
    }
}