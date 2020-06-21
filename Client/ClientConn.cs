using Godot;
using CardGame.Client.Match;
using Godot.Collections;

namespace CardGame.Client {
	
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
				deckList.Add(SetCodes.Alpha_DungeonGuide);
			}

			deckList.Add(SetCodes.Alpha_GuardPuppy);
			deckList.Add(SetCodes.Alpha_WrongWay);
			deckList.Add(SetCodes.Alpha_CounterAttack);
			deckList.Add(SetCodes.Alpha_QuestReward);
			deckList.Add(SetCodes.Alpha_NoviceArcher);
			deckList.Add(SetCodes.Alpha_TrainingTrainer);
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
		public void CreateRoom(string GameID){
			GD.Print("Creating Rooms");
			var gameScene = ResourceLoader.Load("res://Client/Game/Game.tscn") as PackedScene;
			var Room = gameScene.Instance() as Game;
			Room.Name = GameID;
			AddChild(Room);
			Room.SetUp();
		}

	}
}


