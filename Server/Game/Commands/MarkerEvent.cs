namespace CardGame.Server.Room.Commands
{
    public class MarkerEvent: GameEvent
    {
        public readonly ISource Source;
        public readonly GameEvents GameEvent;

        public MarkerEvent(ISource source, GameEvents gameEvent)
        {
            Source = source;
            GameEvent = gameEvent;
        }
        
    }
}