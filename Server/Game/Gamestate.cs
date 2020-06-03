using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CardGame.Server {
		
	public class Gamestate : Reference
	{
		[Signal]
		delegate void RequestTargets();
		
		[Signal]
		public delegate void UnPaused();

		public Unit Target;
		public bool Paused = false;
		public Reference History;
		public int NextCardID = 0;
		public Godot.Collections.Dictionary<int, Player> PlayerMap = new Godot.Collections.Dictionary<int, Player>();
		public List<Player> Players = new List<Player>();
		public Godot.Collections.Dictionary<int, Card> CardCatalog = new Godot.Collections.Dictionary<int, Card>();
		public Unit Attacking;
		public Player TurnPlayer;
		//public Player ActivePlayer;

		public Gamestate(List<Player> players)
		{
			PlayerMap[players[0].Id] = players[0];
			PlayerMap[players[1].Id] = players[1];
			Players = players;
		}

		public void RegisterCard(Card card)
		{
			card.Id = NextCardID;
			CardCatalog[card.Id] = card;
			NextCardID++;
		}
		
		public Card GetCard(int id)
		{
			return CardCatalog[id];
		}

		public void Pause()
		{
			Paused = true;
		}

		public Player GetTurnPlayer()
		{
			return TurnPlayer;
		}

		public void TargetsRequested(int who, List<System.Object> what = default(List<System.Object>))
		{
			
		}

		public void OnTargetsSelected(int what)
		{
			GD.Print("Selecting Target");
			Target = (Unit)CardCatalog[what];
			GD.Print("Target is: ", Target);
			Unpause();
		}

		public void Unpause()
		{
			GD.Print("Paused? ", Paused);
			Paused = false;
			EmitSignal(nameof(UnPaused));
		}

		public Player Player(int player)
		{
			return PlayerMap[player];
		}
		
	}

}
