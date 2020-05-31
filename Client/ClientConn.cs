using Godot;
using System;
using System.Collections.Generic;
using CardGameSharp.Client.Game;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace CardGame.Client {
	
	public class ClientConn : Connection
	{
		const string Ip = "127.0.0.1";
		const int Port = 5000;
		//private List<SetCodes> Decklist = new List<SetCodes>(); // How do we send this info online?
		private NetworkedMultiplayerENet client;
		public Array<SetCodes> DeckList;


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
		// Debug
	
		public override void _Ready() 
		{	
			DeckList = DefaultDeck();
			GetNode("Join").Connect("pressed", this, "Join");
		}
		
		public void Join() {
			
			RemoveChild(GetNode("Join"));
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
			var gameScene = ResourceLoader.Load("res://Client/Game/Game.tscn") as PackedScene;
			var Room = gameScene.Instance() as Game;
			Room.Name = GameID;
			AddChild(Room);
			Room.SetUp(true, new Manual());
		}

	}
}


