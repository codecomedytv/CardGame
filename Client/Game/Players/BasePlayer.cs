using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
    public class BasePlayer: Spatial //, IPlayerView
    {
        private HealthBar HealthBar { get; set; }

        public int Health
        {
            get => HealthBar.Value;
            set => SetHealth(value);
        }
        
        public PlayerView View { get; set; }
        public Zone Deck { get; private set; }
        public Zone Graveyard { get; private set; }
        public Zone Hand { get; private set; }
        public Zone Units { get; private set; }
        public Zone Support { get; private set; }

        public BasePlayer()
        {
            
        }
        public BasePlayer(PlayerView view)
        {
            View = view;
            Units = new Zone(View.Units);
            Support = new Zone(View.Support);
            Hand = new Zone(View.Hand);
            Graveyard = new Zone(View.Graveyard);
            Deck = new Zone(View.Deck);
            HealthBar = View.HealthBar;
        }
        
        private int SetHealth(int health)
        {
            HealthBar.OnHealthChanged(Health - health);
            return health;
        }
    }
}