using CardGame.Client.Game.Zones;

namespace CardGame.Client.Game.Players
{
    public interface IPlayerModelController
    {
        IPlayerView View { get; set; }
        public int Health { get; set; }
        IZone Deck { get; set; }
        IZone Discard { get; set; }
        IZone Hand { get; set; }
        IZone Units { get; set; }
        IZone Support { get; set; }
    }
}