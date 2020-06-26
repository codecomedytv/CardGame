using System;
using Godot;
using System.Collections.Generic;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Zones;

namespace CardGame.Server {

	public class Player : Node, ISource
	{

		public readonly List<SetCodes> DeckList;
		public readonly int Id;

		public States State;
		public Player Opponent;
		public int Health = 8000;
		public bool Ready = false;
		public readonly Zone Deck;
		public readonly Zone Graveyard;
		public readonly Zone Hand;
		public readonly Zone Support;
		public readonly Zone Field;
		public bool IsDisqualified;
		public Func<bool> IsTurnPlayer;
		public Match Match;
		public int Seat;

		[Signal]
		public delegate void TargetSelected();

		public Player()
		{
			
		}

		public void OnTargetSelected(Card card) => EmitSignal(nameof(TargetSelected), card);
		
		public void SetState(States newState)
		{
			State = newState;
			switch (State)
			{
				case States.Idle:
				{
					foreach(var card in Hand) {card.SetCanBeDeployed();}
					foreach(var card in Hand) {card.SetCanBeSet();}
					foreach(var card in Field) {card.SetCanAttack();}
					foreach(var card in Support) {card.SetCanBeActivated();}
					break;
				}
				case States.Active:
				{
					foreach(var card in Support) {card.SetCanBeActivated();}
					break;
				}
			}
		}
		
		public Player(int id, List<SetCodes> deckList)
		{
			DeckList = deckList;
			Id = id;
			// Shuffle = shuffle;
			Deck = new Zone(this);
			Graveyard = new Zone(this);
			Hand = new Zone(this);
			Support = new Zone(this);
			Field = new Zone(this);
		}
		
		public void Shuffle() { /* TODO: Implement Shuffle */ }
		
		public void Draw() => Match.History.Add(new Move(GameEvents.Draw, this, Deck.Top, Hand));
		
		public void Win() => Match.History.Add(new GameOver(this, Opponent)); 
	}
	
}
