using CardGame.Client.Game.Players;
using CardGame.Client.Game.Table;
using Godot;

namespace CardGame.Client.Game
{
    public class Match: Node
    {
        private readonly Catalog Cards = new Catalog();
        private readonly CommandQueue CommandQueue = new CommandQueue();
        private readonly Messenger Messenger = new Messenger();
        private readonly ITableView TableView;
        public IPlayer Player;
        public IPlayer Opponent;

        public Match()
        {
            TableView = new Table3D();
        }

        public Match(ITableView tableView)
        {
            TableView = tableView;
        }

        public override void _Ready()
        {
            AddChild((Node) TableView);
            AddChild(Messenger);
            Messenger.Receiver.Connect(nameof(MessageReceiver.Draw), this, nameof(OnDraw));
            Messenger.CallDeferred("SetReady");
        }

        public void OnDraw(int cardId, bool isOpponent)
        {
            GD.Print($"Draw {cardId} by Opponent? {isOpponent}");
        }
    }
}