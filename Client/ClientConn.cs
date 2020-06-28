using CardGame.Client.Room;
using Godot;
using Godot.Collections;

namespace CardGame.Client {
	
	// ReSharper disable once ClassNeverInstantiated.Global
	public class ClientConn : Connection
	{
		const string Ip = "127.0.0.1";
		const int Port = 5000;
		//private List<SetCodes> Decklist = new List<SetCodes>(); // How do we send this info online?
		public NetworkedMultiplayerENet client;
		public Array<SetCodes> DeckList;

		public override void _Ready()
		{
			DeckList = DefaultDeck();
		}

		// Debug
		public Array<SetCodes> DefaultDeck()
		{
			var deckList = new Array<SetCodes>();
			for (var i = 0; i < 34; i++)
			{
				deckList.Add(SetCodes.AlphaDungeonGuide);
			}

			deckList.Add(SetCodes.AlphaGuardPuppy);
			deckList.Add(SetCodes.AlphaWrongWay);
			deckList.Add(SetCodes.AlphaCounterAttack);
			deckList.Add(SetCodes.AlphaQuestReward);
			deckList.Add(SetCodes.AlphaNoviceArcher);
			deckList.Add(SetCodes.AlphaTrainingTrainer);
			return deckList;
		}

		public void Join() {
			
			client = new NetworkedMultiplayerENet();
			Godot.Error err = client.CreateClient(Ip, Port);
			if(err != Error.Ok) { GD.PushWarning(err.ToString()); }
			err = CustomMultiplayer.Connect("connected_to_server", this, "OnConnected");
			if(err != Error.Ok) { GD.PushWarning(err.ToString()); }
			CustomMultiplayer.Connect("connection_failed", this, "OnFailed");
			CustomMultiplayer.NetworkPeer = client;
		}
		
		public void OnConnected() {
			RpcId(1, "RegisterPlayer", CustomMultiplayer.GetNetworkUniqueId(), DeckList);
		}
		
		public void OnFailed() { GD.Print("Connection Failed"); }

		[Puppet]
		public void CreateRoom(string gameId, int seatPosition){
			var gameScene = (PackedScene) ResourceLoader.Load("res://Client/Room/Game.tscn");
			var room = (Game) gameScene.Instance();
			room.Name = gameId;
			AddChild(room);
			room.SetUp();
		}

	}
}


