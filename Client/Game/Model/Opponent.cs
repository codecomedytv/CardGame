using CardGame;
using CardGame.Client.Match;
using Godot;
using Godot.Collections;
using Card = CardGame.Client.Library.Card.Card;
using Player = CardGame.Client.Match.Player;
using Zone = System.Collections.Generic.List<CardGame.Client.Library.Card.Card>;
using Array = Godot.Collections.Array;
using Library = CardGame.Client.Library.Library;

namespace CardGame.Client.Match
{
	public class Opponent : Reference
	{
		public int Health = 8000;
		public int DeckSize = 0;
		public int HandSize = 0;
		public Zone Field = new Zone();
		public Zone Support = new Zone();
		public Zone Graveyard = new Zone();
		public CardGame.Client.Match.View.Opponent Visual;
		public Dictionary<int, Card> Cards;
		public Player Enemy;
		public Zone Link;

		public Opponent(Dictionary<int, Card> cards)
		{
			Cards = cards;
		}

		public void ShowAttack(int attackerId, int targetId)
		{
			object defender = null;
			var attacker = Cards[attackerId];
			defender = targetId != -1 ? (object) Cards[targetId] : -1;
			Visual.ShowAttack(attacker, defender);
		}

		public void AttackUnit(int attackerId, int defenderId)
		{
			var attacker = Cards[attackerId];
			var defender = Cards[defenderId];
			Visual.AttackUnit(attacker, defender);
		}

		public void AttackDirectly(int attackerId)
		{
			var attacker = Cards[attackerId];
			Visual.AttackDirectly(attacker);
		}

		public void Deploy(Dictionary data)
		{
			var id = (int) data["Id"];
			var setCode = (SetCodes) data["setCode"];
			var card = Library.Library.Fetch(id, setCode);
			card.Id = id;
			Cards[card.Id] = card;
			HandSize -= 1;
			Field.Add(card);
			Visual.Deploy(card);

		}

		public void Bounce(int id)
		{
			var card = Cards[id];
			var c = card;
			Field.Remove(card);
			HandSize += 1;
			Cards.Remove(c.Id);
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
			var setCode = (SetCodes) arg["setCode"];
			if (!(Library.Library.Fetch(id, setCode) is Card card)) return;
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

		public void SetFaceDown()
		{
			HandSize -= 1;
			var blank = Library.Library.Placeholder();
			Support.Add(blank);
			Visual.SetFaceDown(blank);
		}

		public void Draw()
		{
			HandSize += 1;
			DeckSize -= 1;
			Visual.Draw(1, DeckSize);
		}

		public void DestroyUnit(int id)
		{
			var card = Cards[id];
			Field.Remove(card);
			Graveyard.Add(card);
			Visual.DestroyUnit(card);
		}

		public void LoseLife(int lifeLost)
		{
			Health -= lifeLost;
			Visual.LoseLife(lifeLost);
		}

		public void LoadDeck(int deckSize)
		{
			DeckSize = deckSize;
			Visual.LoadDeck(deckSize);
		}
	}
}

