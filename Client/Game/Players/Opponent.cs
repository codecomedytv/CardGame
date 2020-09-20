using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using CardGame.Client.Game.Zones.ModelControllers;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Opponent: BasePlayer
	{
		private HealthBar HealthBar { get; set; }

		public override int Health
		{
			get => HealthBar.Value;
			set => SetHealth(value);
		}

		public override UnitsModel UnitsModel { get; protected set; }
		public override SupportModel SupportModel { get; protected set; }
		public override HandModel HandModel { get; protected set; }
		public override GraveyardModel GraveyardModel { get; protected set; }
		public override DeckModel DeckModel { get; protected set; }

		public override void _Ready()
		{
			UnitsModel = new UnitsModel((Units) GetNode("Units"));
			SupportModel = new SupportModel((Support) GetNode("Support"));
			HandModel = new HandModel( (Hand) GetNode("Hand"));
			GraveyardModel = new GraveyardModel((Graveyard) GetNode("Graveyard"));
			DeckModel = new DeckModel((Deck) GetNode("Deck"));
			HealthBar = (HealthBar) GetNode("HUD/Health");
		}
		
		private int SetHealth(int health)
		{
			HealthBar.OnHealthChanged(Health - health);
			return health;
		}
	}
}
