using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Game.Players
{
	[UsedImplicitly]
	public class Player: BasePlayer
	{
		
		
		private Player() { }
		public Player(PlayerView view): base(view)
		{
		}
	}
}
