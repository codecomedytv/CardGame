using System.Diagnostics;
using System.Security.Cryptography;
using CardGame.Client;
using CardGame.Server;
using Godot;

namespace CardGame
{
    public class TestConnection: Node
    {
        private readonly ServerConn ServerConn = new ServerConn();
        private readonly ClientConn Client1 = new ClientConn();
        private readonly ClientConn Client2 = new ClientConn();

        public override void _Ready()
        {
            GetNode<Button>("Button").Connect("pressed", this, nameof(OnStartPressed));
        }

        public void OnStartPressed()
        {
            GetNode<Button>("Button").Visible = false;
            AddChild(ServerConn);
            AddChild(Client1);
            AddChild(Client2);
            ServerConn.Visible = false;
            ServerConn.Name = "Server";
            Client1.Name = "Client1";
            Client2.Name = "Client2";
            ServerConn.Host();
            Client1.Join();
            Client2.Join();
        }
    }
}