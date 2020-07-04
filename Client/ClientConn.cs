using CardGame.Client.Room;
using Godot;
using Godot.Collections;

namespace CardGame.Client {
	
	// ReSharper disable once ClassNeverInstantiated.Global
	public class ClientConn : Connection
	{
		private const string Ip = "127.0.0.1";
		private const int Port = 5000;
		private readonly CSharpScript Room = (CSharpScript) ResourceLoader.Load("res://Client/Room/Game.cs");
		private DeckList DeckList = new DeckList();
		public NetworkedMultiplayerENet Client;

		public ClientConn() { }
		public ClientConn(CSharpScript room) => Room = room;
		public override void _Ready() {}
		
		public void Join(DeckList deckList = null)
		{
			
			DeckList = deckList ?? DeckList;
			Client = new NetworkedMultiplayerENet();
			var err = Client.CreateClient(Ip, Port);
			if(err != Error.Ok) { GD.PushWarning(err.ToString()); }
			err = CustomMultiplayer.Connect("connected_to_server", this, "OnConnected");
			if(err != Error.Ok) { GD.PushWarning(err.ToString()); }
			CustomMultiplayer.Connect("connection_failed", this, "OnFailed");
			CustomMultiplayer.NetworkPeer = Client;
		}
		
		public void OnConnected() {
			RpcId(1, "RegisterPlayer", CustomMultiplayer.GetNetworkUniqueId(), DeckList);
		}
		
		public void OnFailed() { GD.Print("Connection Failed"); }

		[Puppet]
		public void CreateRoom(string gameId, int seatPosition){
			var room = (Game) Room.New();
			room.Name = gameId;
			AddChild(room);
			room.SetUp();
		}

	}
}


