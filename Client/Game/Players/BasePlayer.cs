using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
    public class BasePlayer: Spatial, IPlayerView
    {
        private HealthBar HealthBar { get; set; }

        public int Health
        {
            get => HealthBar.Value;
            set => SetHealth(value);
        }
        public Zone Deck { get; private set; }
        public Zone Graveyard { get; private set; }
        public Zone Hand { get; private set; }
        public Zone Units { get; private set; }
        public Zone Support { get; private set; }

        public override void _Ready()
        {
            Units = new Zone((Spatial) GetNode("Units"));
            Support = new Zone( (Spatial) GetNode("Support"));
            Hand = new Zone((Spatial) GetNode("Hand"));
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