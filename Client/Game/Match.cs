using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Commands;
using CardGame.Client.Game.Players;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Game
{
	public class Match: Spatial
	{
		[Signal] public delegate void OnExecutionComplete();
		private readonly Catalog Cards = new Catalog();
		private readonly Queue<Command> CommandQueue = new Queue<Command>();
		private readonly CardFactory CardFactory = new CardFactory();
		private readonly GameInput GameInput = new GameInput();
		private readonly Effects Effects = new Effects();
		private readonly Table Table = new Table();
		private CardViewer CardViewer;
		public readonly Messenger Messenger = new Messenger();
		
		// We should add things to the tree before passing them into this constructor..
		// ..if they need to be create beforehand. That way these can be readonly fields.
		public Player Player { get; private set; }
		public Player Opponent { get; private set; }

		public override void _Ready()
		{
			AddChild(Table, true);
			AddChild(Messenger);
			AddChild(GameInput);
			AddChild(Effects);

			CardViewer = (CardViewer) Table.CardViewer;
			Player = new Player((PlayerView) Table.PlayerView, true);
			Opponent = new Player((PlayerView) Table.OpponentView);
			GameInput.User = Player;
			GameInput.Opponent = Opponent;
			
			Messenger.ExecuteEvents = Execute;
			Messenger.QueueEvent = Queue;

			CardFactory.OnCardCreated += GameInput.OnCardCreated;
			CardFactory.OnCardCreated += CardViewer.OnCardCreated;
			CardFactory.OnCardCreated += Cards.OnCardCreated;
			CardFactory.OnCardCreated += OnCardCreated;
			
			GameInput.Deploy = Messenger.DeclareDeploy;
			GameInput.SetCard = Messenger.DeclareSet;
			GameInput.Activate = Messenger.DeclareActivation;
			GameInput.Attack = Messenger.DeclareAttack;
			GameInput.DirectAttack = Messenger.DeclareDirectAttack;
			GameInput.PassPlay = Messenger.DeclarePassPlay;
			GameInput.EndTurn = Messenger.DeclareEndTurn;

			Table.PassPlayPressed = GameInput.OnPassPlayPressed;
			Table.EndTurnPressed = GameInput.OnEndTurnPressed;

			// This check exists mainly to avoid problems in a non-RPC test method
			if (Multiplayer.NetworkPeer != null)
			{
				Messenger.CallDeferred("SetReady");
			}
			else
			{
				GD.PushWarning("No Network Peer Active");
			}

			LoadOpponentDeck();
		}

		private void OnCardCreated(Card card) { AddChild(card); }
		
		private async void Execute()
		{
			while (CommandQueue.Count > 0)
			{
				await CommandQueue.Dequeue().Execute(Effects);
			}
			EmitSignal(nameof(OnExecutionComplete));
			
		}

		private void Queue(CommandId commandId, params object[] args)
		{
			Call(commandId.ToString(), args);
		}
		
		private void LoadOpponentDeck()
		{
			CommandQueue.Enqueue(new OpponentLoadDeck(Opponent, CardFactory));
		}
		
		private void LoadDeck(IDictionary<int, SetCodes> deckList)
		{
			CommandQueue.Enqueue(new LoadDeck(Player, CardFactory, deckList));
		}
		
		public void RevealCard(int id, SetCodes setCode, int zoneIds)
		{
			var card = CardFactory.Create(id, setCode);
			card.OwningPlayer = Opponent; 
			card.Controller = Opponent;
		}
		
		// I wanted to cast these to an int Array (for consistency sake with the RPC) but..
		// ..that does not seem to track well and just causes index errors)
		private void UpdateCard(int id, CardStates state, IEnumerable<int> attackTargetIds, IEnumerable<int> skillTargetIds)
		{
			// Don't ever queue actions like this up again..
			// ..I was updating 40 cards at a time which made our command unnecessairly huge
			Cards[id].Update(state, 
				skillTargetIds.Select(targetId => Cards[targetId]).ToList(), 
				attackTargetIds.Select(targetId => Cards[targetId]).ToList());
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
			// We're always assuming opponent? What about our own activations?
			CommandQueue.Enqueue(new Activate(Opponent, Cards[id], Table.ActivationView));
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
			CommandQueue.Enqueue(new DeclareDirectAttack(Player, Cards[attackerId]));
		}
		
		private void BattleUnit(int attackerId, int defenderId, bool isOpponent)
		{
			CommandQueue.Enqueue(new Battle(Cards[attackerId], Cards[defenderId]));
		}

		private void DirectAttack(int attackerId, bool isOpponent)
		{
			CommandQueue.Enqueue(new DirectAttack(GetPlayer(isOpponent), Cards[attackerId]));
		}

		private void LoseLife(int lifeLost, bool isOpponent)
		{
			CommandQueue.Enqueue(new LoseLife(GetPlayer(isOpponent), lifeLost, Table));
		}
		
		private Player GetPlayer(bool isOpponent)
		{
			return isOpponent ? Opponent : Player as Player;
		}
	}
}
