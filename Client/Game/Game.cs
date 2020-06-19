using Godot;
using System;
using System.Collections.Generic;
using CardGame.Client.Library.Card;
using CardGame.Client.Match;
using CardGame.Client.Match.Model;

namespace CardGame.Client {

	public class Game : Control
	{
		public Gfx Gfx;
		public Sfx Sfx;
		public AudioStreamPlayer BackgroundMusic;
		public Messenger Messenger = new Messenger();
		public Player Player;
		public Opponent Opponent;
		public EventManager EventManager = new EventManager();
		public bool Muted = false;
		public Godot.Collections.Dictionary<int, Card> Cards = new Godot.Collections.Dictionary<int, Card>();
		public List<Card> Link = new List<Card>();

		public override void _Ready()
		{
			Gfx = GetNode<Gfx>("Effects/GFX_Visual");
			Sfx = GetNode<Sfx>("Effects/SFX_Audio");
			BackgroundMusic = GetNode<AudioStreamPlayer>("Effects/BGM_Audio");
		}

		public void SetUp(bool muteMusic)
		{
			
			Muted = muteMusic;
			AddChild(Messenger, true);
			Messenger.CustomMultiplayer = GetParent().Multiplayer;
			Player = new Player(Cards);
			Opponent = new Opponent(Cards);
			Player.Visual = GetNode<Match.View.Player>("Player");
			Opponent.Visual = GetNode<Match.View.Opponent>("Opponent");
			Player.Visual.Setup(Gfx, Sfx);
			Opponent.Visual.Setup(Gfx, Sfx);
			var networkId = CustomMultiplayer.GetNetworkUniqueId();
			Messenger.Id = networkId;
			Sfx.StreamPaused = Muted;
			BackgroundMusic.StreamPaused = Muted;
			Player.Opponent = Opponent;
			Opponent.Enemy = Player;
			Player.Visual.Cards = Cards;
			Opponent.Visual.Cards = Cards;
			EventManager.SetUp(Player, Opponent);
			_Connect(EventManager, "Animated", Gfx, nameof(CardGame.Client.Match.Gfx.StartAnimation));
			_Connect(Messenger, "QueuedEvent", EventManager, "Queue");
			_Connect(Messenger, "ExecutedEvents", EventManager, "Execute");
			_Connect(Messenger, "DisconnectPlayer", GetParent(), "ForceDisconnected");
			//_Connect(Gfx, "tween_all_completed", EventManager, "OnAnimationFinished");

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

