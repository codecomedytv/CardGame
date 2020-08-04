using Godot;

namespace CardGame.Client.Room.View
{
    public class Table : Node
    {
        public Player Player;
        public Player Opponent;

        public override void _Ready()
        {
            Player = GetNode<Player>("Player");
            Opponent = GetNode<Player>("Opponent");
        }

        public override void _Process(float delta)
        {
            if (Godot.Input.IsActionJustPressed("ui_up"))
            {
                OS.WindowFullscreen = !OS.WindowFullscreen;
            }
        }
    }
    
}
