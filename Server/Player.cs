using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Player : Node
	{
		public enum States { Idle, Active, Passive, Acting, Passing }

		public States State = States.Passive;
		private List<Godot.Object> DeckList;
		public readonly int Id;
		public Player Opponent;
		public bool Ready = false;

		[Signal]
		public delegate void PlayExecuted();
		
		[Signal]
		public delegate void PriorityPassed();

		[Signal]
		public delegate void TurnEnded();

		[Signal]
		public delegate void Register();

		[Signal]
		public delegate void Deployed();

		[Signal]
		public delegate void Paused();
		
		public Player() {}
		
		public Player(int id, List<Godot.Object> deckList)
		{
			DeckList = deckList;
			Id = id;
		}

		public void LoadDeck(Gamestate game)
		{
			throw new NotImplementedException();
		}

		public void Shuffle()
		{
			throw new NotImplementedException();
		}

		public void Draw(int drawCount)
		{
			throw new NotImplementedException();
		}

		public void SetPlayableCards()
		{
			throw new NotImplementedException();
		}

		public void Legalize()
		{
			throw new NotImplementedException();
		}

		public void DeclareState()
		{
			throw new NotImplementedException();
		}
	}
}
