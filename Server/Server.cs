using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {
	
	public class Server : Connection
	{
		
		const int Port = 5000;
		private int RoomCount = 0;
		private NetworkedMultiplayerENet server;
		private List<Player> Queue = new List<Player>();
		
		public override void _Ready() 
		{
			Host();
		}
	
		public override void _Process(float delta) 
		{
			if(Queue.Count > 1) { CreateRoom(); }
			
			// For some reason we need to call this manually if we override
			// process otherwise multiplayer won't work!
			Multiplayer.Poll();
		}
		
		public override void _Notification(int Notification)
		{
			if(Notification == NotificationExitTree) 
			{ 
			  server.CloseConnection(); 
			}
		}
		
		[Master]
		public void RegisterPlayer(int ID, List<System.Object> Decklist) 
		{
			Queue.Add(new Player(ID, Decklist));
			GD.Print("Added Player");
		}
		
		private void Host() 
		{
			server = new NetworkedMultiplayerENet();
			Godot.Error err = server.CreateServer(Port);
			if(err != Error.Ok ) { GD.PushWarning(err.ToString()); }
			Multiplayer.NetworkPeer = server;
		}
		
		private void CreateRoom() 
		{
			List<Player> Players = GetPlayers();
			Game Room = new Game(Players);
			RoomCount++;
			Room.Name = RoomCount.ToString();
			AddChild(Room);
			// Add Disqualifcation Method Here
			RpcId(Players[0].ID, "CreateRoom", Room.Name);
			RpcId(Players[1].ID, "CreateRoom", Room.Name);
		}
		
		private List<Player> GetPlayers(int count = 2)
		{
			List<Player> Players = new List<Player>();
			for(int i = 0; i < count; i++){
				Players.Add(Queue[0]);
				Queue.RemoveAt(0);
			}
			return Players;
		}
			
		
	}
	
}
