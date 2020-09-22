using System.Collections.Generic;
using CardGame.Client;
using CardGame.Client.Game;
using CardGame.Server;
using Godot;

namespace CardGame
{
	public class TestConnection: Node
	{
		private readonly IList<Match> Games = new List<Match>();
		private readonly ServerConn ServerConn = new ServerConn();
		private readonly ClientConn Client1 = new ClientConn();
		private readonly ClientConn Client2 = new ClientConn();
		private OptionButton TestSelector;
		private Button RunSelectedTest;
		private Button HostJoinJoin;
		private Button HostJoin;
		private Button Join;

		public override void _Ready()
		{
			GetTree().Connect("node_added", this, nameof(OnNodeAdded));
			HostJoinJoin = GetNode<Button>("Menu/HostJoinJoin");
			HostJoin = GetNode<Button>("Menu/HostJoin");
			Join = GetNode<Button>("Menu/Join");

			TestSelector = GetNode<OptionButton>("Menu/Tests");
			RunSelectedTest = GetNode<Button>("Menu/RunTest");
			RunSelectedTest.Connect("pressed", this, "OnRunSelectedTest");
			HostJoinJoin.Connect("pressed", this, nameof(OnStartPressed));
			HostJoin.Connect("pressed", this, nameof(OnHostJoinPressed));
			Join.Connect("pressed", this, nameof(OnJoinPressed));
			SetUpTests();
		}

		private void OnNodeAdded(Node node) { if (node is Match match) { Games.Add(match); } }

		private void SetUpTests()
		{
			var d = new Directory();
			d.Open("res://Tests/Visual");
			d.ListDirBegin(true);
			var filename = d.GetNext();
			while (filename != "")
			{
				TestSelector.AddItem($"res://Tests/Scripts/Visual/{filename}");
				filename = d.GetNext();
			}
			d.ListDirEnd();
		}

		public void OnRunSelectedTest()
		{
			var file = TestSelector.GetItemText(TestSelector.GetSelectedId());
			var n = GD.Load<CSharpScript>(file);
			Node o = (Node) n.New();
			AddChild(o);
		}

		private void HideButtons()
		{
			GetNode<VBoxContainer>("Menu").Visible = false;
		}

		private async void OnStartPressed()
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
			await ToSignal(GetTree().CreateTimer(0.1F), "timeout");
			ReverseGameVisibility(Games[0]);
		}

		private void ReverseGameVisibility(Spatial game)
		{
			game.Visible = !game.Visible;
			game.GetNode<Control>("Spatial/Table3D/PlayMat/Player/HUD").Visible = game.Visible;
			game.GetNode<Control>("Spatial/Table3D/PlayMat/Opponent/HUD").Visible = game.Visible;
			game.GetNode<Control>("Spatial/Table3D/HUD").Visible = game.Visible;
		}

		private void OnHostJoinPressed()
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

		private void OnJoinPressed()
		{
			HideButtons();
			AddChild(Client1);
			Client1.Name = "Client1";
			Client1.Join();
		}

		public override void _Process(float delta)
		{
			if (!Input.IsActionJustPressed("ui_up")) return;
			GD.Print("swapping games");
			foreach (var game in Games) { ReverseGameVisibility(game); }
		}
	}
}
