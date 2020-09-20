using CardGame.Client.Game.Zones;

namespace CardGame.Client.Game.Players
{
    public interface IPlayerModelController
    {
        public int Health { get; set; }
        IZoneView Deck { get; set; }
        IZoneView Discard { get; set; }
        IZoneView Hand { get; set; }
        IZoneView Units { get; set; }
        IZoneView Support { get; set; }
    }
}