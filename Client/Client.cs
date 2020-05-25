using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Client {
	
	public class Client : Connection
	{
		const string Ip = "127.0.0.1";
		const int Port = 5000;
		private List<System.Object> Decklist; // How do we send this info online?
		private NetworkedMultiplayerENet client;
	
		public Client() {}
		
		public override void _Ready() 
		{	
			GD.Print("Client Ready");
			GetNode("Join").Connect("pressed", this, "Join");
		}
		
		public void Join() {
			
			RemoveChild(GetNode("Join"));
			client = new NetworkedMultiplayerENet();
			Godot.Error err = client.CreateClient(Ip, Port);
			if(err != Error.Ok) { GD.PushWarning(err.ToString()); }
			err = Multiplayer.Connect("connected_to_server", this, "OnConnected");
			if(err != Error.Ok) { GD.PushWarning(err.ToString()); }
			Multiplayer.Connect("connection_failed", this, "OnFailed");
			Multiplayer.NetworkPeer = client;
			GD.Print("Join End");
			GD.Print(client.GetConnectionStatus());
		}
		
		public void OnConnected() {
			GD.Print("Attempting To Register");
			//RpcId(1, "RegisterPlayer", Multiplayer.GetNetworkUniqueId(), Decklist);
		}
		
		public void OnFailed() { GD.Print("Connection Failed"); }
		
		//[Puppet]
		public void CreateRoom(string GameID){
			GD.Print("Creating Room");
		}

	}
}


