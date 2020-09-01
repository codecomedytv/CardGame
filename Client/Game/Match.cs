using System;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using CardGame.Server.Game;
using Godot;
using States = CardGame.Client.Game.Players.States;

namespace CardGame.Client.Game
{
    public class Match: Spatial
    {
        private static int _matchDebugCount = 0;
        private readonly Catalog Cards = new Catalog();
        private readonly CommandQueue CommandQueue = new CommandQueue();
        private readonly CommandFactory CommandFactory = new CommandFactory();
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
                _ => throw new NotSupportedException($"Command {command} has no Counterpart Method")
            };
        }

        private async void Execute(States stateAfterExecution)
        {
            await CommandQueue.Execute();
            var p = (Player) Player;
            p.SetState(stateAfterExecution); // ClientSide States Are Erroneously affecting this (processing etc)
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

        private void OnDraw(int cardId = 0, bool isOpponent = false)
        {
            var command = isOpponent ? CommandFactory.Draw(Opponent) : CommandFactory.Draw(Cards[cardId]);
            CommandQueue.Add(command);
        }

        private void OnCardUpdated(int id, CardStates state, IEnumerable<int> attackTargets, IEnumerable<int> targets)
        {
            var card = Cards[id];
            // card.update
            // use card ids not ints for targets
            // just replace
            card.State = state;
            card.ValidTargets.Clear();
            
            // Just Replace The Lists?
            // Cannot do that because we need to get the cards
            // unless we check via ids?
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
            // We already know our own cards (so far) so we revealed cards default to Opponents;
            var card = CardFactory.Create(id, setCode);
            Opponent.RegisterCard(card);
            AddChild(card);
            card.MouseOvered = GameInput.OnMousedOverCard;
            card.MouseOveredExit = GameInput.OnMousedOverExitCard;
            Cards.Add(id, card);
        }

        private void OnCardDeployed(int id)
        {
            var command = new Deploy(Cards[id]);
            CommandQueue.Add(command);
        }

        private void OnCardSetFaceDown(int id, bool isOpponent)
        {
            var command = isOpponent ? CommandFactory.SetFaceDown(Opponent) : CommandFactory.SetFaceDown(Cards[id]);
            CommandQueue.Add(command);
        }

        private void OnCardActivated(int id, bool isOpponent, int targetId = 0)
        {
            if (!isOpponent) return;
            var command = CommandFactory.Activate(Opponent, Cards[id]);
            CommandQueue.Add(command);
        }

        public void OnCardSentToZone(int cardId, int zoneId)
        {
            // We may want to make this more specific on server-side too
            var command = CommandFactory.SendCardToGraveyard(Cards[cardId]);
            CommandQueue.Add(command);
        }
        
        public void OnOpponentAttackUnit(int attackerId, int defenderId)
        {
            var command = Opponent.Attack(Cards[attackerId], Cards[defenderId]);
            CommandQueue.Add(command);
        }

        public void OnOpponentAttackDirectly(int attackerId)
        {
            var p = (Player) Player;
            var command = p.GetAttackedDirectly(Cards[attackerId]);
            CommandQueue.Add(command);
        }
        
        private void OnUnitBattled(int attackerId, int defenderId, bool isOpponent)
        {
            var command = CommandFactory.Battle(Cards[attackerId], Cards[defenderId]);
            CommandQueue.Add(command);
        }

        private void OnDirectAttack(int attackerId, bool isOpponent)
        {
            var command = GetPlayer(isOpponent).AttackDirectly(Cards[attackerId]);
            CommandQueue.Add(command);
        }

        private void OnLifeLost(int lifeLost, bool isOpponent)
        {
            var command = GetPlayer(isOpponent).LoseLife(lifeLost);
            CommandQueue.Add(command);
        }
        

        private IPlayer GetPlayer(bool isOpponent)
        {
            return isOpponent ? Opponent : Player as IPlayer;
        }
    }
}