using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Client.Game
{
    public class Match: Spatial
    {
        private static int _matchDebugCount = 0;
        private readonly Catalog Cards = new Catalog();
        private readonly Queue<Command> CommandQueue = new Queue<Command>();
        private readonly Messenger Messenger = new Messenger();
        private readonly CardFactory CardFactory;
        private readonly Table Table;
        private Player Player;
        private Opponent Opponent;
        private readonly GameInput GameInput = new GameInput();
        private readonly Tween Gfx = new Tween();

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
            AddChild(Gfx);

            // Change Back Into ControllerModel/View Objects
            Player = (Player) Table.PlayerView;
            Opponent = (Opponent) Table.OpponentView;
            GameInput.User = Player;
            GameInput.Opponent = Opponent;
            
            Messenger.ExecuteEvents = Execute;
            Messenger.QueueEvent = Queue;
            
            GameInput.Deploy = Messenger.DeclareDeploy;
            GameInput.SetCard = Messenger.DeclareSet;
            GameInput.Activate = Messenger.DeclareActivation;
            GameInput.Attack = Messenger.DeclareAttack;
            GameInput.DirectAttack = Messenger.DeclareDirectAttack;
            GameInput.PassPlay = Messenger.DeclarePassPlay;
            GameInput.EndTurn = Messenger.DeclareEndTurn;
            
            Table.GetNode<Button>("Table3D/HUD/EndTurn").Connect("pressed", GameInput, nameof(GameInput.OnEndTurnPressed));
            Table.GetNode<Button>("Table3D/HUD/PassPlay").Connect("pressed", GameInput, nameof(GameInput.OnPassPlayPressed));
            Messenger.CallDeferred("SetReady");

            LoadOpponentDeck();
            DebugCount();
        }

        private void DebugCount()
        {
            // Used For Testing In The Same Editor
            if (_matchDebugCount > 0)
            {
                Visible = false;
                Table.GetNode<Control>("Table3D/HUD").Visible = false;
            }
            _matchDebugCount += 1;    
        }

        private async void Execute()
        {
            while(CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(Gfx); }
        }

        private void Queue(Commands command, params object[] args)
        {
            Call(command.ToString(), args);
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

        private void LoadDeck(Godot.Collections.Dictionary<int, SetCodes> deckList)
        {
            var deck = new System.Collections.Generic.List<Card>();
            foreach (var kv in deckList)
            {
                var card = CardFactory.Create(kv.Key, kv.Value);
                AddChild(card); // Move To CardCatalog?
                Cards.Add(kv.Key, card);
                card.MouseOvered = GameInput.OnMousedOverCard;
                card.MouseOveredExit = GameInput.OnMousedOverExitCard;
                deck.Add(card);
            }
            
            Player.LoadDeck(deck);
        }
        
        public void RevealCard(int id, SetCodes setCode, int zoneIds)
                {
                    // We already know our own cards (so far) so we revealed cards default to Opponents;
                    // Could we pre-send opponent cards ids over? Would it be meaningful if all info remains on server
                    var card = CardFactory.Create(id, setCode);
                    Opponent.RegisterCard(card);
                    AddChild(card);
                    card.MouseOvered = GameInput.OnMousedOverCard;
                    card.MouseOveredExit = GameInput.OnMousedOverExitCard;
                    Cards.Add(id, card);
                }

        private void SetState(States state)
        {
            CommandQueue.Enqueue(new SetState(Player, state));
        }

        private void Draw(int cardId)
        {
            CommandQueue.Enqueue(new Draw(Cards[cardId]));
        }

        private void Draw()
        {
            CommandQueue.Enqueue(new OpponentDraw(Opponent));
        }

        private void UpdateCard(int id, CardStates state, IList<int> attackTargets, IList<int> targets)
        {
            Cards[id].Update(state, targets, attackTargets);
        }

        private void Deploy(int id)
        {
            CommandQueue.Enqueue(new Deploy(Cards[id]));
        }

        private void SetFaceDown(int id)
        {
            CommandQueue.Enqueue(new SetFaceDown(Cards[id]));
        }

        private void SetFaceDown()
        {
            CommandQueue.Enqueue(new OpponentSetFaceDown(Opponent));
        }

        private void Activate(int id, int targetId = 0)
        {
            CommandQueue.Enqueue(new Activate(Opponent, Cards[id]));
        }

        public void SendCardToZone(int cardId, int zoneId)
        {
            CommandQueue.Enqueue(new SendCardToGraveyard(Cards[cardId]));
        }
        
        public void OpponentAttackUnit(int attackerId, int defenderId)
        {
            CommandQueue.Enqueue(new DeclareAttack(Cards[attackerId], Cards[defenderId]));
        }

        public void OpponentAttackDirectly(int attackerId)
        {
            // The Declaration
            CommandQueue.Enqueue(new DeclareDirectAttack(Player, Cards[attackerId]));
        }
        
        private void BattleUnit(int attackerId, int defenderId, bool isOpponent)
        {
            CommandQueue.Enqueue(new Battle(Cards[attackerId], Cards[defenderId]));
        }

        private void DirectAttack(int attackerId, bool isOpponent)
        {
            // Actual Attack
            CommandQueue.Enqueue(new DirectAttack(GetPlayer(isOpponent), Cards[attackerId]));
        }

        private void LoseLife(int lifeLost, bool isOpponent)
        {
            CommandQueue.Enqueue(new LoseLife(GetPlayer(isOpponent), lifeLost));
        }
        
        private IPlayer GetPlayer(bool isOpponent)
        {
            return isOpponent ? Opponent : Player as IPlayer;
        }
    }
}