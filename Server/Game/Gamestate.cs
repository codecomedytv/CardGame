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

		public Unit Target = null;
		public bool Paused = false;
		public Reference History;
		public int NextCardID = 0;
		public Godot.Collections.Dictionary<int, Player> PlayerMap = new Godot.Collections.Dictionary<int, Player>();
		public List<Player> Players = new List<Player>();
		public Godot.Collections.Dictionary<int, Card> CardCatalog = new Godot.Collections.Dictionary<int, Card>();
		public Unit Attacking;
		public Player TurnPlayer;
		public Player ActivePlayer;

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

		public void Begin(Player first)
		{
			TurnPlayer = first;
			ActivePlayer = first;
		}

		public void OnTurnEnd(Player opponent)
		{
			TurnPlayer = opponent;
			ActivePlayer = TurnPlayer;
		}

		public void OnPriorityPassed(Player Opponent)
		{
			
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

		public Player GetActivePlayer()
		{
			return ActivePlayer;
		}

		public void TargetsRequested(int who, List<System.Object> what = default(List<System.Object>))
		{
			
		}

		public void OnTargetsSelected(int what)
		{
			Target = (Unit)CardCatalog[what];
			Unpause();
		}

		public void Unpause()
		{
			Paused = false;
			EmitSignal("Unpaused");
		}

		public Player Player(int player)
		{
			return PlayerMap[player];
		}
		
	}

}
