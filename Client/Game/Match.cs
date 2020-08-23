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
            // Opponent = new Opponent3DView();
            
            AddChild(Messenger);
            Messenger.Receiver.Connect(nameof(MessageReceiver.LoadDeck), this, nameof(OnLoadDeck));
            Messenger.Receiver.Connect(nameof(MessageReceiver.Draw), this, nameof(OnDraw));

            CommandQueue.SubscribeTo(Player);
            // CommandQueue.SubscribeTo(Opponent)
            CommandQueue.SubscribeTo(Messenger.Receiver);
            Messenger.CallDeferred("SetReady");
        }

        private void OnLoadDeck(Dictionary<int, SetCodes> deckList)
        {
            var deck = new System.Collections.Generic.List<Card>();
            foreach (var kv in deckList)
            {
                var card = CardFactory.Create(kv.Key, kv.Value);
                
                // This may be more suitable in something like the card catalog
                // We want to have cards on the global level with a default of 0, 0, 0 so we can translate
                // them easily
                AddChild((Node) card.View);
                
                Cards.Add(kv.Key, card);
                deck.Add(card);
            }
            
            Player.LoadDeck(deck);
        }

        private void OnDraw(int cardId, bool isOpponent)
        {
            if (isOpponent)
            {
                return; // Skip Opponent For Now
            }
            GetPlayer(false).Draw(Cards[cardId]);
        }

        private IPlayer GetPlayer(bool isOpponent)
        {
            return isOpponent ? Opponent : Player;
        }
    }
}