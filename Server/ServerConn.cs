using Godot;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game;

namespace CardGame.Server {
	
	public class ServerConn : Connection
	{
		
		private const int Port = 5000;
		private int RoomCount = 0;
		public NetworkedMultiplayerENet Server;
		private Queue<Player> Queue = new Queue<Player>();
		
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
		
		public override void _Notification(int notification)
		{
			if(notification == NotificationExitTree) 
			{ 
			  Server.CloseConnection(); 
			}
			
			// Have to add this here because Mono doesn't seem to process through all versions
			if(notification == NotificationEnterTree)
			{
				GetTree().Connect("node_added", this, "OnNodeAdded");
				CustomizeChildren();
			}
			
		}
		
		[Master]
		public void RegisterPlayer(int player, List<int> deckList)
		{
			//List<SetCodes> deckCodes = deckList.ConvertAll(SetCodes).ToList();
			Queue.Enqueue(new Player(player, deckList.Select(c => (SetCodes) c).ToList()));
		}
		
		private void Host() 
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
			RpcId(players[0].Id, "CreateRoom", room.Name);
			RpcId(players[1].Id, "CreateRoom", room.Name);
		}
		
		private Players GetPlayers(int count = 2)
		{
			return new Players(Queue.Dequeue(), Queue.Dequeue());
		}
			
		
	}
	
}
