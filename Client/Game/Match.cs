using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Table;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Game
{
    public class Match: Node
    {
        private readonly Catalog Cards = new Catalog();
        private readonly CommandQueue CommandQueue = new CommandQueue();
        private readonly Messenger Messenger = new Messenger();
        private readonly CardFactory CardFactory;
        private readonly ITableView TableView;
        private IPlayer Player;
        private IPlayer Opponent;

        public Match()
        {
            TableView = new Table3D();
            CardFactory = new CardFactory();
        }

        public Match(ITableView tableView)
        {
            TableView = tableView;
        }

        public override void _Ready()
        {
            AddChild((Node) TableView);
            var view = TableView.PlayerView;
            GD.Print(view);
            Player = new Player(TableView.PlayerView); // Has To Come After Adding Table for view reference

            AddChild(Messenger);
            Messenger.Receiver.Connect(nameof(MessageReceiver.LoadDeck), this, nameof(OnLoadDeck));
            Messenger.Receiver.Connect(nameof(MessageReceiver.Draw), this, nameof(OnDraw));
            Messenger.CallDeferred("SetReady");
        }

        private void OnLoadDeck(Dictionary<int, SetCodes> deckList)
        {
            var deck = new System.Collections.Generic.List<Card>();
            foreach (var kv in deckList)
            {
                var card = CardFactory.Create(kv.Key, kv.Value);
                Cards.Add(kv.Key, card);
                deck.Add(card);
            }
            Player.LoadDeck(deck);
        }

        public void OnDraw(int cardId, bool isOpponent)
        {
        }
    }
}