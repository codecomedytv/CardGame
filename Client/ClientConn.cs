using System;
using CardGame.Client.Room;
using CardGame.Client.Room.View;
using Godot;

namespace CardGame.Client {
	
	// ReSharper disable once ClassNeverInstantiated.Global
	public class ClientConn : Connection
	{
		private const string Ip = "127.0.0.1";
		private const int Port = 5000;
		//private readonly PackedScene PlayMat = (PackedScene) GD.Load("res://Client/Room/View/PlayMat.tscn");
		private readonly PackedScene PlayMat = (PackedScene) GD.Load("res://Client/Room/View/3D/Table.tscn");
		private readonly CSharpScript GameType = (CSharpScript) GD.Load("res://Client/Room/Game.cs");
		private readonly Type GameX = typeof(Game);
		private DeckList DeckList = new DeckList();
		public NetworkedMultiplayerENet Client;

		public ClientConn() { }
		public ClientConn(CSharpScript gameType)
		{
			GameType = gameType;
		}

		public override void _Ready() {}
		
		public void Join(DeckList deckList = null)
		{
			
			DeckList = deckList ?? DeckList;
			Client = new NetworkedMultiplayerENet();
			var err = Client.CreateClient(Ip, Port);
			if(err != Error.Ok) { GD.PushWarning(err.ToString()); }
			err = CustomMultiplayer.Connect("connected_to_server", this, nameof(OnConnected));
			if(err != Error.Ok) { GD.PushWarning(err.ToString()); }
			CustomMultiplayer.Connect("connection_failed", this, nameof(OnFailed));
			CustomMultiplayer.NetworkPeer = Client;
		}
		
		public void OnConnected() {
			RpcId(1, "RegisterPlayer", CustomMultiplayer.GetNetworkUniqueId(), DeckList);
		}
		
		public void OnFailed() { GD.Print("Connection Failed"); }

		[Signal]
		public delegate void GameBegan();
		
		[Puppet]
		public void CreateRoom(string gameId, int seatPosition)
		{
			var playMat = (Table) PlayMat.Instance();
			AddChild((Node) playMat);
			var game = (Game) GameType.New(playMat, gameId);
			AddChild(game);
			EmitSignal(nameof(GameBegan));
		}

	}
}


