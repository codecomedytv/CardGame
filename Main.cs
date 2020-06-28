using CardGame.Client;
using CardGame.Server;
using Godot;

namespace CardGame
{
    public class Main : Control
    {
        public bool IsReady;
        public override void _Ready()
        {
            GetNode<Button>("Options/Play").Connect("pressed", this, "Start");
            GetNode<Button>("Options/HostJoin").Connect("pressed", this, "HostJoin");
            GetNode<Button>("Options/Join").Connect("pressed", this, "Join");
        }

        public void Start()
        {
            GetNode<ServerConn>("ScrollContainer/VBoxContainer/Server").Host();
            GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client").Join();
            GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client2").Join();
        }

        public void HostJoin()
        {
            GetNode<ServerConn>("ScrollContainer/VBoxContainer/Server").Host();
            GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client").Join();
        }

        public void Join()
        {
            GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client").Join();
        }


    }
}
