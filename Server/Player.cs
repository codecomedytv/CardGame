using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.States;
using Godot.Collections;

namespace CardGame.Server {

	public class Player : Node, ISource
	{

		public State State;
		public List<SetCodes> DeckList;
		public readonly int Id;
		public Player Opponent;
		public int Health = 8000;
		public bool Ready = false;
		public List<Card> Deck = new List<Card>();
		public List<Card> Graveyard = new List<Card>();
		public List<Card> Hand = new List<Card>();
		public List<Card> Field = new List<Card>();
		public List<Card> Support = new List<Card>();
		public List<Decorator> Tags = new List<Decorator>();
		public bool IsDisqualified;
		public bool IsTurnPlayer = false;
		public Link Link;
		public Battle Battle;
		
		[Signal]
		public delegate void TargetSelected();

		[Signal]
		public delegate void PlayExecuted();
		
		[Signal]
		public delegate void Register();
		public Player() {}
		
		public bool OnDeploy(Unit unit) => State.OnDeploy(unit);
		
		public bool OnAttack(Unit unit, object defender, bool isDirectAttack) => State.OnAttack(unit, defender, isDirectAttack);
		
		public bool OnActivation(Support card, Array<int> targets) => State.OnActivation(card, targets);

		public void OnTargetSelected(Card card) => EmitSignal(nameof(TargetSelected), card);
		
		public bool OnSetFaceDown(Support support) => State.OnSetFaceDown(support);

		public bool OnPriorityPassed() => State.OnPassPlay();

		public bool OnEndTurn() => State.OnEndTurn();

		public void SetState(State newState)
		{
			State = newState;
			State.OnEnter(this);
			DeclarePlay(new SetState(this, State));
		}
		
		public Player(int id, List<SetCodes> deckList)
		{
			DeckList = deckList;
			Id = id;
			// Shuffle = shuffle;
		}

		#region Commands

		public void LoadDeck(Gamestate game)
		{
			foreach (SetCodes setCode in DeckList)
			{
				var card = Library.Create(setCode);
				card.Skill.GameState = game;
				card.Owner = this;
				card.Controller = this;
				card.Zone = Deck;
				game.RegisterCard(card);
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


		public void Shuffle()
		{
			// TODO: Implement Shuffle
		}

		public void DrawCards(int drawCount)
		{
			for (var i = 0; i < drawCount; i++)
			{
				if (Deck.Count == 0)
				{
					Opponent.Win();
				}
				Draw();
			}
		}

		public void Draw()
		{
			DeclarePlay(new Draw(this, this, Deck[Deck.Count-1]));
		}
		
		public void SetTargets(Card selector, List<Card> targets)
		{
			DeclarePlay(new SetTargets(selector, targets));
		}
		

		public bool HasTag(Tag tag) => Tags.Exists(decorator => decorator.Tag == tag);
		
		public void AttackUnit(Unit attacker, Unit defender)
		{
			DeclarePlay(new AttackUnit(attacker, defender));
		}

		public void AttackDirectly(Unit attacker)
		{
			DeclarePlay(new AttackDirectly(attacker));
		}
		
		public void DestroyUnit(Card card)
		{
			// This might be causing problems elsewhere?
			if (card.HasTag(Tag.CannotBeDestroyedByEffect))
			{
				return;
			}
			if (!card.Controller.Field.Contains(card))
			{
				return;
			}
			card.Controller.Field.Remove(card);
			card.Owner.Graveyard.Add(card);
			card.Zone = card.Owner.Graveyard;
			card.EmitSignal(nameof(Card.Exit));
			
			// This is (currently) required to make sure the animations sync
			if(card.Zone == Graveyard)
				DeclarePlay(new DestroyUnits(card));
			else
			{
				Opponent.DeclarePlay(new DestroyUnits(card));
			}
		}

		public void EndTurn() { DeclarePlay(new EndTurn()); }
		
		public void Win() { DeclarePlay(new GameOver(this, Opponent)); }

		#endregion


		public void Move(List<Card> oldZone, Card card, List<Card> newZone)
		{
			oldZone.Remove(card);
			newZone.Add(card);
			card.Zone = newZone;
			card.EmitSignal(nameof(Card.Exit));
		}
	}
	
}
