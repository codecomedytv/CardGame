namespace CardGame.Server.Game.Commands
{
    public class MarkerEvent: GameEvent
    {
        public readonly GameEvents GameEvent;

        public MarkerEvent(GameEvents gameEvent)
        {
            GameEvent = gameEvent;
        }
        
    }
}