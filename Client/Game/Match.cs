using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Players.Player3D;
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
            Player = new Player(TableView.PlayerView); // Has To Come After Adding Table for view reference
            Opponent = new Opponent(TableView.OpponentView);
            
            AddChild(Messenger);
            Messenger.Receiver.Connect(nameof(MessageReceiver.LoadDeck), this, nameof(OnLoadDeck));
            Messenger.Receiver.Connect(nameof(MessageReceiver.Draw), this, nameof(OnDraw));

            CommandQueue.SubscribeTo(Player);
            CommandQueue.SubscribeTo(Opponent);
            CommandQueue.SubscribeTo(Messenger.Receiver);
            Messenger.CallDeferred("SetReady");

            Cards.Add(0, CardFactory.Create(0, SetCodes.NullCard));
            // I don't really know where else to put this!
            var deck = new System.Collections.Generic.List<Card>();
            for (var i = 0; i < 40; i++)
            {
                 var card = CardFactory.Create(0, SetCodes.NullCard);
                 AddChild((Node) card.View);
                 deck.Add(card);
            }
            
            Opponent.LoadDeck(deck);
            
        }

        private void OnLoadDeck(Dictionary<int, SetCodes> deckList)
        {
            var deck = new System.Collections.Generic.List<Card>();
            foreach (var kv in deckList)
            {
                var card = CardFactory.Create(kv.Key, kv.Value);
                AddChild((Node) card.View); // Move To CardCatalog?
                Cards.Add(kv.Key, card);
                deck.Add(card);
            }
            
            Player.LoadDeck(deck);
        }

        private void OnDraw(int cardId = 0, bool isOpponent = false)
        {
            GetPlayer(isOpponent).Draw(Cards[cardId]);
        }

        private IPlayer GetPlayer(bool isOpponent)
        {
            return isOpponent ? Opponent : Player;
        }
    }
}