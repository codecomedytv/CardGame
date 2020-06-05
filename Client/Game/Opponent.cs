using System;
using System.Collections.Generic;
using CardGame;
using CardGame.Client.Match;
using CardGame.Server;
using Godot;
using Godot.Collections;
using Card = CardGame.Client.Library.Card.Card;
using Player = CardGame.Client.Match.Player;
using CardGame.Client.Library;
using Array = Godot.Collections.Array;
using Library = CardGame.Client.Library.Library;

namespace CardGameSharp.Client.Game
{
	public class Opponent : Reference
	{
		public int Health = 8000;
		public int DeckSize = 0;
		public int HandSize = 0;
		public Array<Card> Field = new Array<Card>();
		public Array<Card> Support = new Array<Card>();
		public Array<Card> Graveyard = new Array<Card>();
		public Visual Visual;
		public Dictionary Cards;
		public Player Enemy;
		public List<Card> Link;

		public void SetUp(Dictionary cards)
		{
			Cards = cards;
		}

		public void ShowAttack(Array args)
		{
			object defender = null;
			var attacker = Cards[(int) args[0]];
			if ((int) args[1] != -1)
			{
				defender = Cards[(int) args[1]];
			}
			else
			{
				defender = -1;
			}

			Visual.ShowAttack((Card) attacker, defender);
		}

		public void AttackUnit(Array args)
		{
			var attacker = Cards[(int) args[0]];
			var defender = Cards[(int) args[1]];
			Visual.AttackUnit((Card) attacker, (Card) defender);
		}

		public void AttackDirectly(Array args)
		{
			var attacker = Cards[(int) args[0]];
			Visual.AttackDirectly((Card) attacker);
		}

		public void Deploy(Array args)
		{
			if (!(args[0] is Dictionary data)) return;
			var id = (int) data["Id"];
			var setCode = (SetCodes) data["setCode"];
			var card = Library.Fetch(id, setCode) as Card;
			card.Id = id;
			Cards[card.Id] = card;
			HandSize -= 1;
			Field.Add(card);
			Visual.Deploy(card);

		}

		public void Bounce(Array args)
		{
			var card = Cards[(int) args[0]];
			var c = card as Card;
			Field.Remove((Card) card);
			HandSize += 1;
			Cards.Remove(c.Id);
			Visual.Bounce((Card) card);
		}

		public void Resolve()
		{
			var linked = new Array<Card>();
			foreach (var card in Link)
			{
				if (!card.UnderPlayersControl)
				{
					linked.Add(card);
				}
			}

			Visual.Resolve(linked);
		}

		public void Activate(Array args)
		{
			// Some cards may be triggered effects of already existing so we may use an id
			// check first, then default to creating a new instance
			if (!(args[0] is Dictionary arg)) return;
			var id = (int) arg["Id"];
			var setCode = (SetCodes) arg["setCode"];
			if (!(Library.Fetch(id, setCode) is Card card)) return;
			card.Zone = Card.Zones.Support;
			GD.Print(Support.Count, " is support Count");
			var index = Support.Count - 1 >= 0 ? Support.Count - 1 : 0;
			GD.Print(index, " is index");
			var old = Support[index];
			Support.RemoveAt(index);
			// Should probably remove this discard thing and set it on resolve instead
			Graveyard.Add(card);
			card.Zone = Card.Zones.Discard;
			var targets = new Array<Card>();
			var targs = args[1] as Array;
			if (args.Count == 2 && targs.Count > 0)
			{
				
				foreach (var cardId in (Array<int>) args[1])
				{
					targets.Add((Card) Cards[cardId]);
				}
			}

			Visual.Activate(card, Link, targets);
		}

		public void SetFaceDown(Array args)
		{
			//var card = Cards[(int) args[0]];
			HandSize -= 1;
			var blank = Library.Placeholder();
			Support.Add(blank);
			Visual.SetFaceDown(blank);
		}
		
		// var card = Cards[(int) args[0]];
		// var c = card as Card;
		// Field.Remove((Card) card);
		// HandSize += 1;
		// Cards.Remove(c.Id);
		// Visual.Bounce((Card) card);

		public void Draw(Array args)
		{
			var count = (int) args[0];
			HandSize += count;
			DeckSize -= count;
			Visual.Draw(args, this);
		}

		public void DestroyUnit(Array args)
		{
			var id = (int) args[0];
			var card = Cards[id] as Card;
			Field.Remove((Card) card);
			Graveyard.Add((Card) card);
			Visual.DestroyUnit(card);
		}

		public void LoseLife(Array<int> args)
		{
			Health -= (int) args[0];
			Visual.LoseLife(args[0]);
		}

		public void LoadDeck(int deckSize)
		{
			DeckSize = deckSize;
			Visual.LoadDeck(deckSize);
		}
	}
}

