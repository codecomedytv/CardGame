using System;
using CardGame.Client;
using CardGame.Server;
using Godot;

namespace CardGame
{
    public class Main : Control
    {
        public override void _Ready()
        {
            GetNode<Button>("Options/Play").Connect("pressed", this, "Start");
            GetNode<Button>("Options/HostJoin").Connect("pressed", this, "HostJoin");
            GetNode<Button>("Options/Join").Connect("pressed", this, "Join");
            AddClients();
        }

        private void AddClients(int count = 1)
        {

            var client = new ClientConn();
            var client2 = new ClientConn();
            client.Name = "Client";
            client2.Name = "Client2";
            client.RectMinSize = new Vector2(1920, 1080);
            client2.RectMinSize = new Vector2(1920, 1080);
            client.SizeFlagsHorizontal = (int) SizeFlags.Fill;
            client.SizeFlagsVertical = (int) SizeFlags.Fill;
            client2.SizeFlagsHorizontal = (int) SizeFlags.Fill;
            client2.SizeFlagsVertical = (int) SizeFlags.Fill;
            GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").AddChild(client, true);
            GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").AddChild(client2, true);

            
        }

        public void Start()
        {
            OS.WindowFullscreen = true;
            GetNode<VBoxContainer>("Options").Hide();
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

        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent is InputEventKey key && key.Pressed)
            {
                switch (key.Scancode)
                {
                    case (uint) KeyList.Q:
                    {
                        GetTree().Quit();
                        break;
                    }
                    case (uint) KeyList.F:
                    {
                        OS.WindowFullscreen = !OS.WindowFullscreen;
                        break;
                    }
                }
            }
        }
    }
}
