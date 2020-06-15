﻿using System;
using System.Collections.Generic;
using CardGame.Client.Library.Card;
using CardGame.Client.Match;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Client.Match.View
{
	public class Player : Visual
	{

		public void AutoTarget(Card card)
		{
			foreach (var target in card.ValidTargets)
			{
				QueueCallback((Card)Cards[(int)target], Delay(), "ShowAsValid", true);
			}
		}

		[Signal]
		public delegate void ButtonAction();

		public void SetState(string state)
		{
			switch (state)
			{
				case "Idle":
					EmitSignal(nameof(ButtonAction), "");
					break;
				case "Active":
					EmitSignal(nameof(ButtonAction), "Pass");
					break;
				case "Passive":
					break;
				case "Acting":
					break;
				case "Passing":
					break;
				case "Targeting":
					break;
				default:
					EmitSignal(nameof(ButtonAction), "Wait");
					break;
			}

			var active = GetNode("Active") as Label;
			if (active != null) active.Text = state.ToString();
		}

		public void ShowAttack(Card attacker, object defender)
		{
			attacker.Combat.Show();
			if (defender is Card card)
			{
				card.Combat.Show();
			}
		}
		
		public void Bounce(Card card)
		{
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Hand), 0.3F, Delay());
			QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
			QueueCallback(Hand, Delay(), "add_child", card);
			QueueCallback(Sfx, Delay(), "Play", Sfx.Deploy); // Guess we didn't have a dedicated bounce sfx
			GD.Print();
		}

		public void Resolve(Array<Card> linked)
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
			link.Add(card);
			QueueCallback(card.Link, Delay(), "set_text", link.Count.ToString());
			QueueCallback(card.Link, Delay(0.1F), "set_visible", true);
			QueueCallback(card, Delay(), "FlipFaceUp");
			QueueCallback(Sfx, Delay(), "Play", Sfx.Deploy);
			QueueCallback(card.Back, Delay(0.1F), "hide");
		}

		public void AttackUnit(Card attacker, Card defender)
		{
			var targetPoisition = new Vector2(0, defender.RectGlobalPosition.y + defender.RectScale.y);
			QueueProperty(attacker, "RectGlobalPosition", attacker.RectGlobalPosition, targetPoisition, 0.1F, Delay());
			QueueProperty(attacker, "RectGlobalPosition", targetPoisition, attacker.RectGlobalPosition, .1F, Delay(0.1F));
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(defender, Delay(), "RemoveAura");
			QueueCallback(Sfx, Delay(0.3F), "Play", Sfx.Battle);
		}
		
		public void AttackDirectly(Card attacker)
		{
			var targetPosition = new Vector2(0, attacker.RectGlobalPosition.y - 70);
			QueueProperty(attacker, "RectGlobalPosition", attacker.RectGlobalPosition, targetPosition, 0.3F, Delay());
			QueueProperty(attacker, "RectGlobalPosition", targetPosition, attacker.RectGlobalPosition, 0.3F, Delay(0.3F));
			Animate.AddDelay(0.3F);
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(Sfx, Delay(0.3F), "Play", Sfx.Battle);
		}
		
		public void ReadyCard(Card card) => QueueCallback(card, Delay(), "Ready");
		
		public void UnreadyCard(Card card) => QueueCallback(card, Delay(), "Exhaust");
	

		public void Deploy(Card card)
		{
			
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Units), 0.3F, Delay());
			QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
			QueueCallback(Units, Delay(), "add_child", card);
			QueueCallback(card, Delay(), "FlipFaceUp");
			QueueCallback(Sfx, Delay(), "Play", Sfx.Deploy);
			
		}

		public void SetFaceDown(Card card)
		{
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Support), 0.3F, Delay());
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
			QueueCallback(Damage, Delay(0.5F), "set_self_modulate", invisible);
		}
		
		public void DestroyUnit(Card card)
		{
			QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
			QueueCallback(Discard, Delay(), "add_child", card);
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, Discard.RectGlobalPosition, 0.3F,
				Delay());
		}
		
		public void LoadDeck(int deckSize)
		{
			QueueCallback(Deck, Delay(0.3F), "set_text", deckSize.ToString());
		}

		public void Draw(Card card, int deckSize)
		{
			var destination = FuturePosition(Hand);
			Hand.AddChild((Card)card);
			card.RectGlobalPosition = Deck.RectGlobalPosition;
			card.TurnInvisible();
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, destination, 0.2F, Delay(0.2F));
			QueueCallback(card, Delay(), "TurnVisible");
			QueueCallback(Deck, Delay(), "set_text", deckSize);
			QueueCallback(Sfx, Delay(), "Play", Sfx.Draw);
			QueueCallback(this, Delay(0.2F), "Sort", Hand);
			
		}

		public void BeginTurn()
		{
			GD.PushWarning("Delete Unused Method");
		}

		public void EndTurn()
		{
			GD.PushWarning("Delete Unused Method");
		}
	}
}
