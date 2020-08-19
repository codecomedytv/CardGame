using CardGame.Client.Game.Players;
using CardGame.Client.Game.Table;
using Godot;

namespace CardGame.Client.Game
{
    public class Match: Node
    {
        public IPlayer Player;
        public IPlayer Opponent;
        public Catalog Cards;

        public ITableView TableView; // Not sure how to incorporate this correctly, just a means of building views?
        public CommandQueue CommandQueue;
        public Messenger Messenger;
        public Input Input; // Not sure if Input is really necessary, though we do need to handle events from cards
    }
}