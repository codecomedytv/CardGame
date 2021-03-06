using Godot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CardGame.Server.Game;

namespace CardGame.Server {
	
	// ReSharper disable once ClassNeverInstantiated.Global
	public class ServerConn : Connection
	{
		
		private const int Port = 5000;
		private int RoomCount = 0;
		public NetworkedMultiplayerENet Server;
		private readonly Queue<Player> Queue = new Queue<Player>();
		
		public override void _Ready() 
		{
		}
	
		public override void _Process(float delta) 
		{
			if(Queue.Count > 1) { CreateRoom(); }
			Multiplayer.Poll();
		}
		
		public override void _Notification(int notification)
		{
			switch (notification)
			{
				case NotificationExitTree:
					Server?.CloseConnection();
					break;
				case NotificationEnterTree:
					GetTree().Connect(NodeAdded, this, nameof(OnNodeAdded));
					CustomizeChildren();
					break;
			}
		}
		
		[Master]
		public void RegisterPlayer(int player, Godot.Collections.Array<SetCodes> deckList)
		{
			Queue.Enqueue(new Player(player, deckList.ToList()));
		}

		public void Host() 
		{
			Server = new NetworkedMultiplayerENet();
			var err = Server.CreateServer(Port);
			if(err != Error.Ok ) { GD.PushWarning(err.ToString()); }
			CustomMultiplayer.NetworkPeer = Server;
		}
		
		private void CreateRoom() 
		{
			var players = GetPlayers();
			var room = new Match(players);
			RoomCount++;
			room.Name = RoomCount.ToString();
			AddChild(room);
			// Add disqualification Method Here
			foreach (var player in players) { RpcId(player.Id, "CreateRoom", room.Name);}
		}
		
		private Players GetPlayers(int count = 2)
		{
			return new Players(Queue.Dequeue(), Queue.Dequeue());
		}
			
		
	}
	
}
