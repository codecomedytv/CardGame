using Godot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Zones;
using CardGame.Server.States;

namespace CardGame.Server {

	public class Player : Node, ISource
	{

		public readonly List<SetCodes> DeckList;
		public readonly int Id;
		public State State;
		public Player Opponent;
		public int Health = 8000;
		public bool Ready = false;
		public readonly Zone Deck;
		public readonly Zone Graveyard;
		public readonly Zone Hand;
		public readonly Zone Support;
		public readonly Zone Field;
		public bool IsDisqualified;
		public bool IsTurnPlayer = false;
		public Match Match;
		public int Seat;

		[Signal]
		public delegate void TargetSelected();

		[Signal]
		public delegate void PlayExecuted();

		public Player()
		{
			
		}
		
		public bool OnDeploy(Unit unit) => State.OnDeploy(unit);
		
		public bool OnAttack(Unit unit, Unit defender) => State.OnAttack(unit, defender);

		public bool OnDirectAttack(Unit attacker) => State.OnDirectAttack(attacker);
		
		public bool OnActivation(Support card, Card target) => State.OnActivation(card, target);

		public void OnTargetSelected(Card card) => EmitSignal(nameof(TargetSelected), card);
		
		public bool OnSetFaceDown(Support support) => State.OnSetFaceDown(support);

		public bool OnPriorityPassed() => State.OnPassPlay();

		public bool OnEndTurn() => State.OnEndTurn();

		public void SetState(State newState)
		{
			State = newState;
			State.OnEnter(this);
			// TODO: We've removed the state game event since it was largely unnecessary but we will still..
			// TODO: need a way to inform the client
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
		
		public void DeclarePlay(Command command)
		{
			command.Execute();
			EmitSignal(nameof(PlayExecuted), this, command);
		}
		
		public void Shuffle() { /* TODO: Implement Shuffle */ }
		
		public void Draw() => DeclarePlay(new Move(this, Deck.Top, Hand));
		
		public void Win() { DeclarePlay(new GameOver(this, Opponent)); }
		
		
	}
	
}
