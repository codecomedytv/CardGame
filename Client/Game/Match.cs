using CardGame.Client.Game.Players;
using CardGame.Client.Game.Table;
using Godot;

namespace CardGame.Client.Game
{
    public class Match: Node
    {
        private readonly ITableView TableView;
        public IPlayer Player;
        public IPlayer Opponent;
        public Catalog Cards;
        public CommandQueue CommandQueue;
        public Messenger Messenger;
        
        // Input is part of Player: IPlayerController. Remove this.
        public Input Input;

        public Match()
        {
            TableView = Table3D.CreateInstance();
        }

        public Match(ITableView tableView)
        {
            TableView = tableView;
        }

        public override void _Ready()
        {
            Name = "Match";
            AddChild((Node) TableView);
        }
    }
}