namespace CardGame.Server.Game.Commands
{
    public abstract class Event: Godot.Object
    {
        protected GameEvents GameEvent = GameEvents.NoOp;
        public GameEvents Identity => GameEvent;

        protected Event()
        {
            
        }
        public abstract void Execute();
        public abstract void Undo();
    }
}