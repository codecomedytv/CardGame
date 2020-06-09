using System;
using System.Collections.Generic;
using CardGame.Client.Library.Card;
using CardGame.Client.Match;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Client.Match.View
{
	public class Opponent : Visual
	{
		public void ShowAttack(Card attacker, object defender)
		{
			attacker.Combat.Show();
			if (defender is Card card)
			{
				card.Combat.Show();
				History.AddLine($"Enemy's {attacker} your {card}");
			}
			else
			{
				History.AddLine($"Enemy's {attacker} attacked directly");
			}
		}
		
		public void Bounce(Card card)
		{
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Hand), 0.3F, Delay());
			QueueCallback(History, Delay() + 0.3F, "AddLine", $"{card} was returned to Enemy's Hand");
			QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
			QueueCallback(Hand, Delay(), "add_child", card);
			QueueCallback(Sfx, Delay(), "Play", Sfx.Deploy); // Guess we didn't have a dedicated bounce sfx
			QueueCallback(card, Delay(), "FlipFaceDown");
			var fake = Library.Library.Placeholder();
			QueueCallback(Hand, Delay(), "remove_child", card);
			QueueCallback(Hand, Delay(), "add_child", fake);
			QueueCallback(card, Delay(), "QueueFree");
		}

		public void Resolve(IEnumerable<Card> linked)
		{
			foreach (var card in linked)
			{
				QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
				QueueCallback(Discard, Delay(), "add_child", card);
				// Should add a check for unit based effects (for some reason?)
				QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, Discard.RectGlobalPosition, 0.3F, Delay());
			}
		}

		public void Activate(Card card, List<Card> link, Array<Card> targets)
		{
			
			Support.GetChild(0).Free();
			Support.AddChild(card);
			card.Back.Visible = true;
			link.Add(card);
			QueueCallback(card.Link, Delay(), "set_text", link.Count.ToString());
			QueueCallback(card.Link, Delay(0.1F), "set_visible", true);
			QueueCallback(card, Delay(), "FlipFaceUp");
			QueueCallback(Sfx, Delay(), "Play", Sfx.Deploy);
			QueueCallback(card.Back, Delay(0.1F), "hide");
			QueueCallback(History, Delay(0.1F), "AddLine", $"Enemy activated {card}");
			if (targets.Count != 0)
			{
				QueueCallback(History, Delay(0.1F), "AddLine", $"Targeting: {targets}");
			}
		}

		public void AttackUnit(Card attacker, Card defender)
		{
			var targetPosition = new Vector2(0, defender.RectGlobalPosition.y - defender.RectScale.y);
			QueueProperty(attacker, "RectGlobalPosition", attacker.RectGlobalPosition, targetPosition, 0.1F, Delay());
			QueueProperty(attacker, "RectGlobalPosition", targetPosition, attacker.RectGlobalPosition, .1F, Delay(0.1F));
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(defender, Delay(), "RemoveAura");
			QueueCallback(Sfx, Delay(0.3F), "Play", Sfx.Battle);
		}
		
		public void AttackDirectly(Card attacker)
		{
			var targetPosition = new Vector2(0, 70);
			QueueProperty(attacker, "RectGlobalPosition", attacker.RectGlobalPosition, targetPosition, 0.3F, Delay());
			QueueProperty(attacker, "RectGlobalPosition", targetPosition, attacker.RectGlobalPosition, 0.3F, Delay(0.3F));
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(History, Delay(0.1F), "DirectAttack", Who, attacker);
			QueueCallback(Sfx, Delay(0.3F), "Play", Sfx.Battle);
		}

		public void ReadyCards(Array args)
		{
			foreach (var id in args)
			{
				QueueCallback(Cards[(int)id], Delay(), "Ready");
			}
		}

		public void UnreadyCards(Array args)
		{
			foreach(var id in args)
			{
				QueueCallback((Card)Cards[(int)id], Delay(), "Exhaust");
			}
		}

		public void Deploy(Card card)
		{
			Hand.RemoveChild(Hand.GetChild(0));
			Hand.AddChild(card);
			Sort(Hand);
			card.FlipFaceDown();
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Units), 0.3F, Delay());
			QueueCallback(History, Delay() + 0.3F, "AddLine", $"Enemy Deployed {card}");
			QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
			QueueCallback(Units, Delay(), "add_child", card);
			QueueCallback(card, Delay(), "FlipFaceUp");
			QueueCallback(Sfx, Delay(), "Play", Sfx.Deploy);
			
		}

		public void SetFaceDown(Card card)
		{
			Hand.RemoveChild(Hand.GetChild(0));
			Hand.AddChild(card);
			Sort(Hand);
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Support), 0.3F, Delay());
			QueueCallback(History, Delay(), "AddLine", "Enemy set a FaceDown Card");
			QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
			QueueCallback(Support, Delay(), "add_child", card);
			GD.Print("setting ", card.ToString());
			QueueCallback(card, Delay(), "FlipFaceDown");
			QueueCallback(Sfx, Delay(), "Play", Sfx.SetFaceDown);
		}
		
		public void LoseLife(int damageTaken)
		{
			Damage.Text = "-" + damageTaken.ToString();
			Life.Text = (Life.Text.ToInt() - (int) damageTaken).ToString();
			var visible = Damage.Modulate + new Color(0, 0, 0, 255);
			var invisible = Damage.Modulate - new Color(0, 0, 0, 255);
			QueueCallback(Damage, Delay(), "set_self_modulate", visible);
			QueueCallback(History, Delay(), "AddLine", $"Opponent took {damageTaken} damage");
			QueueCallback(Damage, Delay(0.5F), "set_self_modulate", invisible);
		}
		
		public void DestroyUnit(Card card)
		{
			QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
			QueueCallback(Discard, Delay(), "add_child", card);
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, Discard.RectGlobalPosition, 0.3F,
				Delay());
			QueueCallback(History, Delay(), "AddLine", $"Enemy's {card} was destroyed");
		}
		
		public void LoadDeck(int deckSize)
		{
			QueueCallback(Deck, Delay(0.3F), "set_text", deckSize.ToString());
		}

		public void BeginTurn()
		{
			QueueCallback(History, Delay(), "AddLine", "Enemy's Turn Has Begun");
		}

		public void EndTurn()
		{
			QueueCallback(History, Delay() ,"AddLine", "Enemy Ended Their Turn");
		}


		public void Draw(int args, int deckSize)
		{
			var destination = FuturePosition(Hand);
			var card = Library.Library.Placeholder();
			Hand.AddChild((Card)card);
			card.RectGlobalPosition = Deck.RectGlobalPosition;
			card.TurnInvisible();
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, destination, 0.2F, Delay(0.2F));
			QueueCallback(card, Delay(), "TurnVisible");
			QueueCallback(Deck, Delay(), "set_text", deckSize);
			QueueCallback(Sfx, Delay(), "Play", Sfx.Draw);
			QueueCallback(this, Delay(0.2F), "Sort", Hand);
			QueueCallback(History, Delay(), "AddLine", "Enemy drew a card");
		}
	}
}
