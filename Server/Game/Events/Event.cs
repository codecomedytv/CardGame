namespace CardGame.Server.Game.Events
{
    public abstract class Event: Godot.Object
    {
        protected GameEvents GameEvent = GameEvents.NoOp;
        public GameEvents Identity => GameEvent;

        protected Event()
        {
            
        }
        
    }
}