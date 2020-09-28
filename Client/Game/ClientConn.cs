using CardGame.Client.Game;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client {
	
	[UsedImplicitly]
	public class ClientConn : Connection
	{
		private const string Ip = "127.0.0.1";
		private const int Port = 5000;
		private DeckList DeckList = new DeckList();
		private NetworkedMultiplayerENet Client;
		private readonly Match Match;

		public ClientConn() => Match = new Match();
		public ClientConn(Match match) => Match = match;

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

		public void OnConnected()
		{
			RpcId(1, "RegisterPlayer", CustomMultiplayer.GetNetworkUniqueId(), DeckList.ToArray());
		}

		public void OnFailed() { GD.Print("Connection Failed"); }
		
		[Puppet]
		public void CreateRoom(string gameId)
		{
			Match.Name = gameId;
			AddChild(Match);
		}
	}
}


