using System;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Game.Players
{
    [UsedImplicitly]
    public class PlayerView: Spatial
    {
        [CanBeNull] private Sprite EnergyIcon;
        private HealthBar HealthBar;
        private Sprite DefenseIcon;
        public Spatial Deck { get; private set; }
        public Spatial Graveyard { get; private set; }
        public Spatial Units { get; private set; }
        public Spatial Support { get; private set; }
        public Spatial Hand { get; private set; }

        public override void _Ready()
        {
            Deck = (Spatial) GetNode("Deck");
            Graveyard = (Spatial) GetNode("Graveyard");
            Hand = (Spatial) GetNode("Hand");
            Units = (Spatial) GetNode("Units");
            Support = (Spatial) GetNode("Support");
            HealthBar = (HealthBar) GetNode("HUD/Health");
            DefenseIcon = (Sprite) GetNode("HUD/Defend");
            EnergyIcon = (Sprite) GetNodeOrNull("HUD/EnergyIcon");
        }

        public void Defend()
        {
            DefenseIcon.Visible = true;
        }

        public void StopDefending()
        {
            DefenseIcon.Visible = false;
        }

        public void OnStateChanged(States state)
        {
            if (EnergyIcon != null)
            {
                EnergyIcon.Modulate = (state == States.Idle || state == States.Active) ? Colors.Gold : Colors.Black;
            }
        }

        public void OnHealthChanged(int lifeLost)
        {
            HealthBar.OnHealthChanged(lifeLost);
        }
    }
}