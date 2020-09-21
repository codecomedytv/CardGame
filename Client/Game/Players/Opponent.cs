using CardGame.Client.Game.Zones;
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

		public override Zone Units { get; protected set; }
		public override Zone Support { get; protected set; }
		public override Zone Hand { get; protected set; }
		public override Zone Graveyard { get; protected set; }
		public override Zone Deck { get; protected set; }

		public override void _Ready()
		{
			Units = new Zone((Spatial) GetNode("Units"));
			Support = new Zone((Spatial) GetNode("Support"));
			Hand = new Zone( (Spatial) GetNode("Hand"));
			Graveyard = new Zone((Spatial) GetNode("Graveyard"));
			Deck = new Zone((Spatial) GetNode("Deck"));
			HealthBar = (HealthBar) GetNode("HUD/Health");
		}
		
		private int SetHealth(int health)
		{
			HealthBar.OnHealthChanged(Health - health);
			return health;
		}
	}
}
