using System;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client.Game.Players
{
	public class Player: IPlayer
	{
		private readonly IPlayerView View;

		public Player(IPlayerView view)
		{
			View = view;
		}
		
		public void Connect(Declaration addCommand)
		{
			View.Connect(addCommand);
		}
		
		public void LoadDeck(IEnumerable<Card> deck)
		{
			foreach (var card in deck)
			{
				View.AddCardToDeck(card);
			}
		}

		public void Draw(Card card)
		{
			View.Draw(card);
		}

		public void Discard(Card card)
		{
			throw new System.NotImplementedException();
		}

		public void Deploy(Card card)
		{
			throw new System.NotImplementedException();
		}

		public void Set(Card card)
		{
			throw new System.NotImplementedException();
		}
	}
}
