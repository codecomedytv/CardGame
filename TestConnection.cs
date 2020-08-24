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
        private Button HostJoinJoin;
        private Button HostJoin;
        private Button Join;

        public override void _Ready()
        {
            HostJoinJoin = GetNode<Button>("HostJoinJoin");
            HostJoin = GetNode<Button>("HostJoin");
            Join = GetNode<Button>("Join");

            HostJoinJoin.Connect("pressed", this, nameof(OnStartPressed));
            HostJoin.Connect("pressed", this, nameof(OnHostJoinPressed));
            Join.Connect("pressed", this, nameof(OnJoinPressed));
        }

        public void HideButtons()
        {
            HostJoinJoin.Visible = false;
            HostJoin.Visible = false;
            Join.Visible = false;
        }

        public void OnStartPressed()
        {
            HideButtons();
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

        public void OnHostJoinPressed()
        {
            HideButtons();
            AddChild(ServerConn);
            AddChild(Client1);
            ServerConn.Visible = false;
            ServerConn.Name = "Server";
            Client1.Name = "Client1";
            ServerConn.Host();
            Client1.Join();
        }

        public void OnJoinPressed()
        {
            HideButtons();
            AddChild(Client1);
            Client1.Name = "Client1";
            Client1.Join();
        }
    }
}