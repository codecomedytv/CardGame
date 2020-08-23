using System;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;
using Object = Godot.Object;

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
		
		public void Connect(Declaration addCommand)
		{
			View.Connect(addCommand);
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
			Model.Draw(card.Model);
			View.Draw(card.View);
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
