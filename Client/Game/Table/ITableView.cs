using CardGame.Client.Game.Players;

namespace CardGame.Client.Game.Table
{
    public interface ITableView
    {
        // Table Views are created manually in the editor
        // Their main purpose is to reach into their children and pull out the Player and Opponent View
        // who both have also reached into their own scenes to pull out their container views
        
        IPlayerView PlayerView { get; set; }
        IPlayerView OpponentView {get; set;}
    }
}