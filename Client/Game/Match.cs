﻿using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
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
        private readonly Table Table;
        private IPlayer Player;
        private IPlayer Opponent;
        private GameInput GameInput = new GameInput();

        public Match()
        {
            Table = new Table();
            CardFactory = new CardFactory();
        }
        
        public override void _Ready()
        {
            AddChild(Table);
            AddChild(Messenger);

            Player = Table.PlayerView; // Has To Come After Adding Table for view reference
            Opponent = Table.OpponentView;
            GameInput.User = (Player) Player;

            Messenger.Receiver.Connect(nameof(MessageReceiver.LoadDeck), this, nameof(OnLoadDeck));
            Messenger.Receiver.Connect(nameof(MessageReceiver.Draw), this, nameof(OnDraw));

            var player = (Player) Player;
            CommandQueue.Connect(nameof(CommandQueue.SetState), player, nameof(player.SetState));

            CommandQueue.SubscribeTo(Player);
            CommandQueue.SubscribeTo(Opponent);
            CommandQueue.SubscribeTo(Messenger.Receiver);
            
            
            Messenger.CallDeferred("SetReady");

            LoadOpponentDeck();
            
        }

        private void LoadOpponentDeck()
        {
            // Begin Loading Opponent Deck
            Cards.Add(0, CardFactory.Create(0, SetCodes.NullCard));
            // I don't really know where else to put this!
            var deck = new System.Collections.Generic.List<Card>();
            for (var i = 0; i < 40; i++)
            {
                var card = CardFactory.Create(0, SetCodes.NullCard);
                AddChild(card); // Need to put this into a central area
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
                AddChild(card); // Move To CardCatalog?
                Cards.Add(kv.Key, card);
                GameInput.SubscribeTo(card);
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