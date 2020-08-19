namespace CardGame.Client.Game.Players
{
    public interface IPlayerView
    {
        void DisplayName(string name);
        void DisplayHealth(int health);
    }
}