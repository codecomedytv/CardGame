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
            var card = CardGame.Client.Cards.Library.Fetch(1, SetCodes.AlphaDungeonGuide);
            AddChild(card);
            var t = new Tween();
            AddChild(t);
            var c = new Control();
            // var c = new TextureRect();
            // c.RectMinSize = new Vector2(200, 200);
            // c.StretchMode = TextureRect.StretchModeEnum.ScaleOnExpand;
            // c.RectSize = new Vector2(200, 200);
            // c.Texture = (Texture) GD.Load("res://Assets/CardArt/coin.png");
            AddChild(c);
            t.InterpolateProperty(c, "rect_global_position", card.RectGlobalPosition, new Vector2(500, 500), 10.0F);
            t.Start();
            //card.RectGlobalPosition = new Vector2(500, 500);
        }

        private void AddClients(int count = 1)
        {
    
            var server = new ServerConn();
            var client = new ClientConn();
            var client2 = new ClientConn();
            server.Name = "Server";
            client.Name = "Client";
            client2.Name = "Client2";
            client.RectMinSize = new Vector2(1920, 1080);
            client2.RectMinSize = new Vector2(1920, 1080);
            client.SizeFlagsHorizontal = (int) SizeFlags.Fill;
            client.SizeFlagsVertical = (int) SizeFlags.Fill;
            client2.SizeFlagsHorizontal = (int) SizeFlags.Fill;
            client2.SizeFlagsVertical = (int) SizeFlags.Fill;
            server.Visible = false;
            GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").AddChild(server, true);
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
