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
	
		// public Client() {}
		
		public override void _Ready() 
		{	
//			CustomMultiplayer = new MultiplayerAPI();
//			CustomMultiplayer.SetRootNode(this); 
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
			GD.Print(String.Format("{0} has connected from {1} and {2}", this, GetCustomMultiplayer(), client));
			RpcId(1, "RegisterPlayer", CustomMultiplayer.GetNetworkUniqueId(), Decklist);
		}
		
		public void OnFailed() { GD.Print("Connection Failed"); }
		
		[Puppet]
		public void CreateRoom(string GameID){
			GD.Print("Creating Room");
		}

	}
}


