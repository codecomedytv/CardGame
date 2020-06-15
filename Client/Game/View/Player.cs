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
	public class Player : Visual
	{

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
		
		public void LoadDeck(int deckSize)
		{
			QueueCallback(Deck, Delay(0.3F), "set_text", deckSize.ToString());
		}

		public void Draw(Card card, int deckSize)
		{
			card.TurnInvisible();
			Hand.AddChild((Card)card);
			Sort(Hand);
			var destination = card.RectGlobalPosition;
			card.RectGlobalPosition = Deck.RectGlobalPosition;
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, destination, 0.2F, Delay(0.2F));
			QueueCallback(card, Delay(), "TurnVisible");
			QueueCallback(Deck, Delay(), "set_text", deckSize);
			QueueCallback(Sfx, Delay(), "Play", Sfx.Draw);
			QueueCallback(this, Delay(0.2F), "Sort", Hand);
			
		}
		
	}
}
