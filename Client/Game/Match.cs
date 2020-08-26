using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using CardGame.Server.Game;
using Godot;

namespace CardGame.Client.Game
{
    public class Match: Spatial
    {
        private static int _matchDebugCount = 0;
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
            AddToGroup("games");
            Table = new Table();
            CardFactory = new CardFactory();
        }
        
        public override void _Ready()
        {
            AddChild(Table, true);
            AddChild(Messenger);
            AddChild(GameInput);

            Player = Table.PlayerView; // Has To Come After Adding Table for view reference
            Opponent = Table.OpponentView;
            GameInput.User = (Player) Player;

            Messenger.Receiver.Connect(nameof(MessageReceiver.LoadDeck), this, nameof(OnLoadDeck));
            Messenger.Receiver.Connect(nameof(MessageReceiver.Draw), this, nameof(OnDraw));
            Messenger.Receiver.Connect(nameof(MessageReceiver.UpdateCard), this, nameof(OnCardUpdated));
            Messenger.Receiver.Connect(nameof(MessageReceiver.Deploy), this, nameof(OnCardDeployed));
            Messenger.Receiver.Connect(nameof(MessageReceiver.RevealCard), this, nameof(OnCardRevealed));
            Messenger.Receiver.Connect(nameof(MessageReceiver.SetFaceDown), this, nameof(OnCardSetFaceDown));
            Messenger.Receiver.Connect(nameof(MessageReceiver.Activate), this, nameof(OnCardActivated));
            Messenger.Receiver.Connect(nameof(MessageReceiver.SendCardToZone), this, nameof(OnCardSentToZone));
            Messenger.Receiver.Connect(nameof(MessageReceiver.BattleUnit), this, nameof(OnUnitBattled));
            Messenger.Receiver.Connect(nameof(MessageReceiver.OpponentAttackUnit), this, nameof(OnOpponentAttackUnitQueued));
            
            GameInput.Connect(nameof(GameInput.Deploy), Messenger.Sender, nameof(MessageSender.DeclareDeploy));
            GameInput.Connect(nameof(GameInput.SetCard), Messenger.Sender, nameof(MessageSender.DeclareSet));
            GameInput.Connect(nameof(GameInput.Activate), Messenger.Sender, nameof(MessageSender.DeclareActivation));
            GameInput.Connect(nameof(GameInput.Attack), Messenger.Sender, nameof(MessageSender.DeclareAttack));
            GameInput.Connect(nameof(GameInput.PassPlay), Messenger.Sender, nameof(MessageSender.DeclarePassPlay));
            GameInput.Connect(nameof(GameInput.EndTurn), Messenger.Sender, nameof(MessageSender.DeclareEndTurn));
            

            Table.GetNode<Button>("Table3D/HUD/EndTurn").Connect("pressed", GameInput, nameof(GameInput.OnEndTurnPressed));
            Table.GetNode<Button>("Table3D/HUD/PassPlay").Connect("pressed", GameInput, nameof(GameInput.OnPassPlayPressed));

            var player = (Player) Player;
            CommandQueue.Connect(nameof(CommandQueue.SetState), player, nameof(player.SetState));

            CommandQueue.SubscribeTo(Player);
            CommandQueue.SubscribeTo(Opponent);
            CommandQueue.SubscribeTo(Messenger.Receiver);
            
            
            Messenger.CallDeferred("SetReady");

            LoadOpponentDeck();
            
            // Debug
            if (_matchDebugCount > 0)
            {
                this.Visible = false;
                Table.GetNode<Control>("Table3D/HUD").Visible = false;
            }
            _matchDebugCount += 1;
            
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

        private void OnLoadDeck(Godot.Collections.Dictionary<int, SetCodes> deckList)
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

        private void OnCardUpdated(int id, CardStates state, IEnumerable<int> attackTargets, IEnumerable<int> targets)
        {
            var card = Cards[id];
            card.State = state;
            card.ValidTargets.Clear();
            foreach (var target in targets)
            {
                card.ValidTargets.Add(Cards[target]);
            }

            card.ValidAttackTargets.Clear();

            foreach (var defender in attackTargets)
            {
                card.ValidAttackTargets.Add(Cards[defender]);
            }
        }

        public void OnCardRevealed(int id, SetCodes setCode, int zoneIds)
        {
            var card = CardFactory.Create(id, setCode);
            AddChild(card);
            GameInput.SubscribeTo(card);
            Cards.Add(id, card);
        }

        private void OnCardDeployed(int id, SetCodes setCode, bool isOpponent)
        {
            // Setcode Arg is Legacy, we use a specialized reveal command instead if new card
            GetPlayer(isOpponent).Deploy(Cards[id]);
        }

        private void OnCardSetFaceDown(int id, bool isOpponent)
        {
            // Setcode Arg is Legacy, we use a specialized reveal command instead if new card
            GetPlayer(isOpponent).SetFaceDown(Cards[id]);
        }

        private void OnCardActivated(int id, SetCodes setCode, int positionInLink, bool isOpponent, int targetId = 0)
        {
            GetPlayer(isOpponent).Activate(Cards[id]);
        }

        public void OnCardSentToZone(int cardId, int zoneId, bool isOpponent)
        {
            // We may want to make this more specific on server-side too
            GetPlayer(isOpponent).SendCardToGraveyard(Cards[cardId]);
        }
        
        public void OnOpponentAttackUnitQueued(int attackerId, int defenderId)
        {
            Opponent.Attack(Cards[attackerId], Cards[defenderId]);
        }
        
        private void OnUnitBattled(int attackerId, int defenderId, bool isOpponent)
        {
            GD.Print("Battle!");
        }
        

        private IPlayer GetPlayer(bool isOpponent)
        {
            return isOpponent ? Opponent : Player;
        }
    }
}