namespace CardGame.Server.Game.Events
{
    public class EndTurn: Event
    {
        public readonly ISource Source;

        public EndTurn(ISource source)
        {
            Identity = GameEvents.EndTurn;
            Source = source;
        }


    }
}