using Godot;
using System;
using System.Collections.Generic;
using CardGame.Client.Library.Card;
using CardGame.Client.Match;
using CardGameSharp.Client.Game;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Environment = Godot.Environment;

namespace CardGame.Client {

	public class Game : Control
	{
		public Label WinLoseNote;
		public Gfx Gfx;
		public Sfx Sfx;
		public AudioStreamPlayer BackgroundMusic;
		public History History;
		public Messenger Messenger = new Messenger();
		public Player Player = new Player();
		public Opponent Opponent = new Opponent();
		public EventManager EventManager = new EventManager();
		public GameInput GameInput;
		public bool Muted = false;
		public Dictionary Cards = new Dictionary();
		public List<Card> Link = new List<Card>();

		public override void _Ready()
		{
			WinLoseNote = GetNode<Label>("WIN_LOSE");
			Gfx = GetNode<Gfx>("Effects/GFX_Visual");
			Sfx = GetNode<Sfx>("Effects/SFX_Audio");
			BackgroundMusic = GetNode<AudioStreamPlayer>("Effects/BGM_Audio");
			History = GetNode<History>("History");
		}

		public void SetUp(bool muteMusic, GameInput gameInput)
		{
			
			Muted = muteMusic;
			GameInput = gameInput;
			AddChild(Messenger, true);
			Messenger.CustomMultiplayer = GetParent().Multiplayer;
			Player.Visual = GetNode<Visual>("Player");
			Opponent.Visual = GetNode<Visual>("Opponent");
			Player.Visual.Setup(Gfx, Sfx, History, (int) Gfx.Who.Player);
			Opponent.Visual.Setup(Gfx, Sfx, History, (int) Gfx.Who.Opponent);
			var networkId = CustomMultiplayer.GetNetworkUniqueId();
			Messenger.Id = networkId;
			Sfx.StreamPaused = Muted;
			BackgroundMusic.StreamPaused = Muted;
			Player.SetUp(Cards);
			Opponent.SetUp(Cards);
			Player.Link = Link;
			Opponent.Link = Link;
			Player.Opponent = Opponent;
			Opponent.Enemy = Player;
			Player.Visual.Cards = Cards;
			Opponent.Visual.Cards = Cards;
			EventManager.SetUp(Player, Opponent, GameInput);
			GameInput.SetUp(Player, Opponent, networkId, Messenger, Cards);
			if(GameInput is Manual input)
			{
				input.Interact = GetNode<Interact>("Interact");
				Player.Interact = input.Interact;
				Player.Input = input;
				foreach (var card in Cards)
				{
					if (Cards[(int) card] is Card c) c.Interact = GetNode<Interact>("Interact");
				}
			}
			AddChild(GameInput, true);
			GetNode<Button>("Background/EndTurn").Connect("pressed", GameInput, "OnEndTurn");
			GetNode<Button>("Background/Pass").Connect("pressed", GameInput, "PassPriority");
			Player.Visual.Connect("ButtonAction", GetNode<Button>("Background/Pass"), "set_text");
			_Connect(EventManager, "CommandRequested", GameInput, "Command");
			_Connect(EventManager, "Animated", Gfx, nameof(CardGameSharp.Client.Game.Gfx.StartAnimation));
			_Connect(Player, "PlayerWon", this, "Win");
			_Connect(Player, "PlayerLost", this, "Lose");
			_Connect(Messenger, "QueuedEvent", EventManager, "Queue");
			_Connect(Messenger, "ExecutedEvents", EventManager, "Execute");
			_Connect(Messenger, "DisconnectPlayer", GetParent(), "ForceDisconnected");
			_Connect(Gfx, "tween_all_completed", EventManager, "OnAnimationFinished");

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

		public void Win()
		{
			Gfx.InterpolateCallback(BackgroundMusic, Gfx.TotalDelay((int) Gfx.Who.Player), "Stop");
			Gfx.InterpolateCallback(Sfx, Gfx.AddDelay(0.2F, (int) Gfx.Who.Player), "Victory");
			Gfx.InterpolateCallback(WinLoseNote, Gfx.TotalDelay((int) Gfx.Who.Player), "SetText", "YOU WIN!");
			Gfx.InterpolateCallback(WinLoseNote, Gfx.TotalDelay((int) Gfx.Who.Player), "SetVisible", true);
		}

		public void Lose()
		{
			Gfx.InterpolateCallback(BackgroundMusic, Gfx.TotalDelay((int) Gfx.Who.Player), "Stop");
			Gfx.InterpolateCallback(Sfx, Gfx.AddDelay(0.2F, (int) Gfx.Who.Player), "Defeat");
			Gfx.InterpolateCallback(WinLoseNote, Gfx.TotalDelay((int) Gfx.Who.Player), "SetText", "YOU LOSE!");
			Gfx.InterpolateCallback(WinLoseNote, Gfx.TotalDelay((int) Gfx.Who.Player), "SetVisible", true);
		}
	}
	
}

