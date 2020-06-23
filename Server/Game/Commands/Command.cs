namespace CardGame.Server.Game.Commands
{
    public abstract class Command: Godot.Object
    {
        public Command()
        {
            
        }
        public abstract void Execute();
        public abstract void Undo();
    }
}