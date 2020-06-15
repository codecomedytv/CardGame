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
		public void LoadDeck(int deckSize)
		{
			QueueCallback(Deck, Delay(0.3F), "set_text", deckSize.ToString());
		}
		
		public void Draw(int args, int deckSize)
		{
			var card = Library.Library.Placeholder();
			card.TurnInvisible();
			Hand.AddChild((Card)card);
			Sort(Hand);
			var destination = card.RectGlobalPosition;
			card.RectGlobalPosition = Deck.RectGlobalPosition;
			card.TurnInvisible();
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, destination, 0.2F, Delay(0.2F));
			QueueCallback(card, Delay(), "TurnVisible");
			QueueCallback(Deck, Delay(), "set_text", deckSize);
			QueueCallback(Sfx, Delay(), "Play", Sfx.Draw);
			QueueCallback(this, Delay(0.2F), "Sort", Hand);
		}
	}
}