using Godot;
using System;
using CardGame.Client.Match;
using CardGameSharp.Client.Game;
using Godot.Collections;
using Array = Godot.Collections.Array;

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
		public EventManager EventManager = new EventManager();
		public GameInput GameInput;
		public bool Muted = false;
		public Dictionary Cards;
		public Array Link = new Array();

		public override void _Ready()
		{
			// Temporary Code
			string id = CustomMultiplayer.GetNetworkUniqueId().ToString();
			GD.Print(id + ": Created Room " + Name);
			// End Temporary Code

			WinLoseNote = GetNode<Label>("WIN_LOSE");
			Gfx = GetNode<Gfx>("Effects/GFX_Visual");
			Sfx = GetNode<Sfx>("Effects/SFX_Audio");
			BackgroundMusic = GetNode <AudioStreamPlayer>("Effects/BGM_Audio");
			History = GetNode<History>("History");
		}
		
	}

}
