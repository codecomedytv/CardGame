using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Events;
using CardGame.Server.States;
using Godot.Collections;

namespace CardGame.Server {

	public class Player : Node, ISource
	{

		private readonly List<SetCodes> DeckList;
		public readonly int Id;
		public State State;
		public Player Opponent;
		public int Health = 8000;
		public bool Ready = false;
		public List<Card> Deck = new List<Card>();
		public List<Card> Graveyard = new List<Card>();
		public List<Card> Hand = new List<Card>();
		public List<Card> Field = new List<Card>();
		public List<Card> Support = new List<Card>();
		public bool IsDisqualified;
		public bool IsTurnPlayer = false;
		public Match Match;

		[Signal]
		public delegate void TargetSelected();

		[Signal]
		public delegate void PlayExecuted();
	
		public Player() {}
		
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
		}


		public void LoadDeck(Match match)
		{
			foreach (SetCodes setCode in DeckList)
			{
				var card = Library.Create(setCode);
				card.Skill.Match = match;
				card.Owner = this;
				card.Controller = this;
				card.Zone = Deck;
				match.RegisterCard(card);
				Deck.Add(card);
			}

			DeclarePlay(new LoadDeck(Deck.ToList()));
		}

		public void DeclarePlay(GameEvent gameEvent)
		{
			if (gameEvent is ICommand command)
			{
				command.Execute();
			}
			EmitSignal(nameof(PlayExecuted), this, gameEvent);
		}
		
		public void Shuffle() { /* TODO: Implement Shuffle */ }
		
		public void Draw() => DeclarePlay(new Move(this, Deck[Deck.Count-1], Hand));
		
		public void AttackUnit(Unit attacker, Unit defender) => DeclarePlay(new AttackUnit(attacker, defender));
		
		public void AttackDirectly(Unit attacker) => DeclarePlay(new AttackDirectly(attacker));

		public void Win() { DeclarePlay(new GameOver(this, Opponent)); }
		
		
	}
	
}
