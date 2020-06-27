namespace CardGame.Server.Game.Events
{
    public class EndTurn: Event
    {
        public readonly ISource Source;

        public EndTurn(ISource source)
        {
            GameEvent = GameEvents.EndTurn;
            Source = source;
        }


    }
}