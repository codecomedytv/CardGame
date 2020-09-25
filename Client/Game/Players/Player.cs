using System;
using CardGame.Client.Game.Zones;


namespace CardGame.Client.Game.Players
{
    public class Player: Godot.Object
    {
        private event Action<int> OnHealthChanged;
   
        private int InternalHealth = 8000;
        public int Health
        {
            get => InternalHealth;
            set => SetHealth(value);
        }

        public readonly PlayerView View;
        public readonly PlayerState PlayerState = new PlayerState();
        public readonly Zone Deck = new Zone();
        public readonly Zone Graveyard = new Zone();
        public readonly Zone Hand  = new Zone();
        public readonly Zone Units  = new Zone();
        public readonly Zone Support  = new Zone();
        public readonly bool IsUser;

        private Player() {}
        public Player(PlayerView view, bool isUser = false)
        {
            IsUser = isUser;
            View = view;
            PlayerState.StateChanged += View.OnStateChanged;
            OnHealthChanged += View.OnHealthChanged;
        }
        
        private int SetHealth(int health)
        {
            OnHealthChanged?.Invoke(InternalHealth - health);
            InternalHealth = health;
            return health;
        }
    }
}