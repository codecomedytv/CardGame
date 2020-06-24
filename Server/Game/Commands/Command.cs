namespace CardGame.Server.Game.Commands
{
    public abstract class Command: Godot.Object
    {
        protected GameEvents GameEvent = GameEvents.NoOp;
        public GameEvents Identity => GameEvent;

        public Command()
        {
            
        }
        public abstract void Execute();
        public abstract void Undo();
    }
}