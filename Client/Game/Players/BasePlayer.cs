﻿using CardGame.Client.Game.Zones;
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
        
        public readonly PlayerState PlayerState = new PlayerState();

        public PlayerView View { get; private set; }
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
            // We only care about this as a Player, we just essentially ignore it when working with the opponent
            // even though technically the opponent has it
            PlayerState.StateChanged += View.OnStateChanged;
        }
        
        private int SetHealth(int health)
        {
            HealthBar.OnHealthChanged(Health - health);
            return health;
        }
    }
}