using System.Collections.Generic;
using CardGame;
using CardGame.Client.Match;
using CardGame.Server;
using Godot;
using Godot.Collections;
using Card = CardGame.Client.Library.Card.Card;
using Player = CardGame.Client.Match.Player;
using CardGame.Client.Library;
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
		public Godot.Collections.Dictionary<int, Card> Cards;
		public Player Enemy;
		public List<Card> Link;

		public void SetUp(Godot.Collections.Dictionary<int, Card> cards)
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

			Visual.ShowAttack(attacker, defender);
		}

		public void AttackUnit(Array args)
		{
			var attacker = Cards[(int) args[0]];
			var defender = Cards[(int) args[1]];
			Visual.AttackUnit(attacker, defender);
		}

		public void AttackDirectly(Array args)
		{
			var attacker = Cards[(int) args[0]];
			Visual.AttackDirectly(attacker);
		}

		public void Deploy(Array args)
		{
			if (!(args[0] is Dictionary data)) return;
			var id = (int) data["Id"];
			var setCode = (SetCodes) data["SetCode"];
			if (Library.Fetch(id, setCode) is Card card)
			{
				card.Id = id;
				Cards[card.Id] = card;
				HandSize -= 1;
				Field.Add(card);
			}

			Visual.Deploy(args);

		}

		public void Bounce(Array args)
		{
			var card = Cards[(int) args[0]];
			Field.Remove(card);
			HandSize += 1;
			Cards.Remove(card.Id);
			Visual.Bounce(card);
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
			var setCode = (SetCodes) arg["SetCode"];
			if (!(Library.Fetch(id, setCode) is Card card)) return;
			card.Zone = Card.Zones.Support;
			var old = Support[Support.Count - 1];
			Support.RemoveAt(Support.Count - 1);
			// Should probably remove this discard thing and set it on resolve instead
			Graveyard.Add(card);
			card.Zone = Card.Zones.Discard;
			var targets = new Array<Card>();
			if (args.Count == 2)
			{
				foreach (var cardId in (Array<int>) args[1])
				{
					targets.Add(Cards[cardId]);
				}
			}

			Visual.Activate(card, Link, targets);
		}

		public void SetFaceDown(object _stuff)
		{
			HandSize -= 1;
			var blank = Library.Placeholder();
			Support.Add(blank);
			Visual.SetFaceDown(null);
		}

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
			var card = Cards[id];
			Field.Remove(card);
			Graveyard.Add(card);
			Visual.DestroyUnit(args);
		}

		public void LoseLife(Array<int> args)
		{
			Health -= (int) args[0];
			Visual.LoseLife(args);
		}

		public void LoadDeck(Array args)
		{
			var deckSize = (int) args[0];
			DeckSize = deckSize;
			Visual.LoadDeck(args);
		}
	}
}

