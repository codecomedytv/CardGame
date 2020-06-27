using CardGame.Client;
using Godot;

namespace CardGame
{
    public class Main : Control
    {
        public bool IsReady;
        public override void _Ready()
        {
            GetNode<Button>("Play").Connect("pressed", this, "Start");
        }

        public void Start()
        {
            GD.Print("Game Begins");
            GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client").Join();
            GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client2").Join();
        }


    }
}
