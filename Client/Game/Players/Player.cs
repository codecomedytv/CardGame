using System.Collections.Generic;
using System.Diagnostics;
using CardGame.Client.Game.Cards;
using Godot;
using WAT;

namespace CardGame.Client.Game.Players
{
	public class Player: IPlayer
	{
		private readonly IPlayerModel Model;
		private readonly IPlayerView View;

		public Player(IPlayerView view)
		{
			Model = new PlayerModel();
			View = view;
		}
		public void ConnectCommands(CommandQueue commandQueue)
		{
			throw new System.NotImplementedException();
		}

		public void LoadDeck(IEnumerable<Card> deck)
		{
			foreach (var card in deck)
			{
				View.AddCardToDeck(card.View);
			}
		}

		public void Draw(Card card)
		{
			throw new System.NotImplementedException();
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
