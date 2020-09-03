using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Opponent: Spatial, IPlayer
	{
		private Sprite DefendingIcon { get; set; }
		private HealthBar HealthBar { get; set; }

		public int Health
		{
			get => HealthBar.Value;
			set => SetHealth(value);
		}
		public Units Units { get; private set; }
		public Support Support { get; private set; }
		public Hand Hand { get; private set; }
		public Graveyard Graveyard { get; private set; }
		public Deck Deck { get; private set; }

		public override void _Ready()
		{
			Units = (Units) GetNode("Units");
			Support = (Support) GetNode("Support");
			Hand = (Hand) GetNode("Hand");
			Graveyard = (Graveyard) GetNode("Graveyard");
			Deck = (Deck) GetNode("Deck");
			DefendingIcon = (Sprite) GetNode("HUD/Defending");
			HealthBar = (HealthBar) GetNode("HUD/Health");
		}
		
		private int SetHealth(int health)
		{
			HealthBar.OnHealthChanged(Health - health);
			return health;
		}

		public void StopDefending()
		{
			DefendingIcon.Visible = false;
		}
		
		public void Defend()
		{
			DefendingIcon.Visible = true;
		}

		public void ClearDirectAttackingDefense()
		{
			DefendingIcon.Visible = false;
		}
	}
}
