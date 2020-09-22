using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
    public class BasePlayer: Spatial
    {
        private HealthBar HealthBar { get; set; }

        public int Health
        {
            get => HealthBar.Value;
            set => SetHealth(value);
        }
        
        public PlayerView View { get; private set; }
        protected States BackingState; 
        public States State 
        { 
            // This should access a PlayerState Object and return that Object's internals
            get => BackingState;
            set => SetState(value);
        }
        public readonly Zone Deck = new Zone();
        public readonly Zone Graveyard = new Zone();
        public readonly Zone Hand  = new Zone();
        public readonly Zone Units  = new Zone();
        public readonly Zone Support  = new Zone();
        
        protected BasePlayer() { }

        protected BasePlayer(PlayerView view)
        {
            View = view;
            HealthBar = View.HealthBar;
        }

        protected virtual States SetState(States value)
        {
            return BackingState;
        }
        
        private int SetHealth(int health)
        {
            HealthBar.OnHealthChanged(Health - health);
            return health;
        }
    }
}