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

		public override Units Units { get; protected set; }
		public override Support Support { get; protected set; }
		public override Hand Hand { get; protected set; }
		public override Graveyard Graveyard { get; protected set; }
		public override DeckModel DeckModel { get; protected set; }

		public override void _Ready()
		{
			Units = new Units((UnitsView) GetNode("Units"));
			Support = new Support((SupportView) GetNode("Support"));
			Hand = new Hand( (HandView) GetNode("Hand"));
			Graveyard = new Graveyard((GraveyardView) GetNode("Graveyard"));
			DeckModel = new DeckModel((DeckView) GetNode("Deck"));
			HealthBar = (HealthBar) GetNode("HUD/Health");
		}
		
		private int SetHealth(int health)
		{
			HealthBar.OnHealthChanged(Health - health);
			return health;
		}
	}
}
