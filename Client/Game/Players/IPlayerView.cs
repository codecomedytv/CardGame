namespace CardGame.Client.Game.Players
{
    public interface IPlayerView
    {
        string Username { get; set; }
        int Health { get; set; }
    }
}