using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Game.Players
{
    [UsedImplicitly]
    public class PlayerView: Spatial
    {
        public HealthBar HealthBar;
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
            // Get State For Player (maybe a nodeornull?)
        }

        public void OnStateChanged(States state)
        {
            var energyIcon = GetNodeOrNull("HUD/EnergyIcon");
            if (energyIcon != null && energyIcon is Sprite energy)
            {
                if (state == States.Idle || state == States.Active)
                {
                    energy.Modulate = Colors.Gold;
                }
                else
                {
                    energy.Modulate = Colors.Black;
                }
            }
        }
    }
}