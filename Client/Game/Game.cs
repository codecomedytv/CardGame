using Godot;

namespace CardGame.Client.Game {

	public class Game : Control
	{
		private readonly Messenger Messenger = new Messenger();

		public override void _Ready()
		{
		}

		public void SetUp()
		{
			AddChild(Messenger, true);
			Messenger.CustomMultiplayer = GetParent().Multiplayer;
			var networkId = CustomMultiplayer.GetNetworkUniqueId();
			Messenger.Id = networkId;
			Messenger.CallDeferred("SetReady");

		}

		public void _Connect(Godot.Object emitter, string signal, Godot.Object receiver, string method)
		{
			var err = emitter.Connect(signal, receiver, method);
			if (err != Error.Ok)
			{
				GD.PushWarning(err.ToString());
			}
		}

	}
	
}

