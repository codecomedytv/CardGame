using Godot;

namespace CardGame.Client.Room.View
{
    public class PlayMat: Control
    {
        public Control Player;
        public Control Opponent;

        public override void _Ready()
        {
            Player = GetNode<Control>("Player");
            Opponent = GetNode<Control>("Opponent");
        }
    }
}