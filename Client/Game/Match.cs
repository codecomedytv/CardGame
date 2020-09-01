using System;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
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
        private Player Player;
        private Opponent Opponent;
        private readonly GameInput GameInput = new GameInput();

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
            var t = new Tween();
            Table.AddChild(t);
            CommandQueue.Gfx = t;

            Player = (Player) Table.PlayerView; // Has To Come After Adding Table for view reference
            Opponent = (Opponent) Table.OpponentView;
            GameInput.User = Player;
            GameInput.Opponent = Opponent;
            
            Messenger.ExecuteEvents = this.Execute;
            Messenger.QueueEvent = this.Queue;
            
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
            
            // Debug
            if (_matchDebugCount > 0)
            {
                this.Visible = false;
                Table.GetNode<Control>("Table3D/HUD").Visible = false;
            }
            _matchDebugCount += 1;
            
        }

        [Signal]
        public delegate void Unpack();

        private void Queue(Commands command, params object[] args) // What happens if we emit params to params?
        {
            var methodName = GetCommandName(command);
            if (methodName == "")
            {
                GD.PushWarning($"No Method Found For {command}");
                return;
            }
            Connect(nameof(Unpack), this, methodName, null, (uint) ConnectFlags.Oneshot);
            EmitSignal(nameof(Unpack), args);
        }

        private string GetCommandName(Commands command)
        {
            return command switch
            {
                Commands.Draw => nameof(OnDraw),
                Commands.LoadDeck => nameof(OnLoadDeck),
                Commands.UpdateCard => nameof(OnCardUpdated),
                Commands.Deploy => nameof(OnCardDeployed),
                Commands.RevealCard => nameof(OnCardRevealed),
                Commands.SetFaceDown => nameof(OnCardSetFaceDown),
                Commands.Activate => nameof(OnCardActivated),
                Commands.SendCardToZone => nameof(OnCardSentToZone),
                Commands.BattleUnit => nameof(OnUnitBattled),
                Commands.OpponentAttackUnit => nameof(OnOpponentAttackUnit),
                Commands.OpponentAttackDirectly => nameof(OnOpponentAttackDirectly),
                Commands.DirectAttack => nameof(OnDirectAttack),
                Commands.LoseLife => nameof(OnLifeLost),
                Commands.ResolveCard => "",
                Commands.Trigger => "",
                Commands.GameOver => "",
                Commands.BounceCard => "",
                Commands.TargetRequested => "",
                Commands.SetState => nameof(OnStateSet),
                _ => throw new NotSupportedException($"Command {command} has no Counterpart Method")
            };
        }

        private void Execute()
        { 
            CommandQueue.Execute();
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
                card.MouseOvered = GameInput.OnMousedOverCard;
                card.MouseOveredExit = GameInput.OnMousedOverExitCard;
                deck.Add(card);
            }
            
            Player.LoadDeck(deck);
        }

        private void OnStateSet(States state)
        {
            GD.Print($"Setting State To {state}");
            CommandQueue.Add(new SetState(Player, state));
        }

        private void OnDraw(int cardId)
        {
            CommandQueue.Add(new Draw(Cards[cardId]));
        }

        private void OnDraw()
        {
            CommandQueue.Add(new OpponentDraw(Opponent));
        }

        private void OnCardUpdated(int id, CardStates state, IList<int> attackTargets, IList<int> targets)
        {
            Cards[id].Update(state, targets, attackTargets);
        }

        public void OnCardRevealed(int id, SetCodes setCode, int zoneIds)
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

        private void OnCardDeployed(int id)
        {
            CommandQueue.Add(new Deploy(Cards[id]));
        }

        private void OnCardSetFaceDown(int id)
        {
            CommandQueue.Add(new SetFaceDown(Cards[id]));
        }

        private void OnCardSetFaceDown()
        {
            CommandQueue.Add(new OpponentSetFaceDown(Opponent));
        }

        private void OnCardActivated(int id, int targetId = 0)
        {
            CommandQueue.Add(new Activate(Opponent, Cards[id]));
        }

        public void OnCardSentToZone(int cardId, int zoneId)
        {
            CommandQueue.Add(new SendCardToGraveyard(Cards[cardId]));
        }
        
        public void OnOpponentAttackUnit(int attackerId, int defenderId)
        {
            CommandQueue.Add(new DeclareAttack(Cards[attackerId], Cards[defenderId]));
        }

        public void OnOpponentAttackDirectly(int attackerId)
        {
            // The Declaration
            CommandQueue.Add(new DeclareDirectAttack(Player, Cards[attackerId]));
        }
        
        private void OnUnitBattled(int attackerId, int defenderId, bool isOpponent)
        {
            CommandQueue.Add(new Battle(Cards[attackerId], Cards[defenderId]));
        }

        private void OnDirectAttack(int attackerId, bool isOpponent)
        {
            // Actual Attack
            CommandQueue.Add(new DirectAttack(GetPlayer(isOpponent), Cards[attackerId]));
        }

        private void OnLifeLost(int lifeLost, bool isOpponent)
        {
            CommandQueue.Add(new LoseLife(GetPlayer(isOpponent), lifeLost));
        }
        

        private IPlayer GetPlayer(bool isOpponent)
        {
            return isOpponent ? Opponent : Player as IPlayer;
        }
    }
}