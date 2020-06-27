namespace CardGame.Server.Game.Commands
{
    public class EndTurn: Event
    {
        public readonly ISource Source;

        public EndTurn(ISource source)
        {
            GameEvent = GameEvents.EndTurn;
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