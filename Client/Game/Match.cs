using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
	public class Match: Spatial
	{
		private readonly Catalog Cards = new Catalog();
		private readonly Queue<Command> CommandQueue = new Queue<Command>();
		private readonly Messenger Messenger = new Messenger();
		private readonly CardFactory CardFactory = new CardFactory();
		private readonly GameInput GameInput = new GameInput();
		private readonly Tween Gfx = new Tween();
		private readonly Table Table = new Table();
		private readonly BattleSystem BattleSystem = new BattleSystem();
		private Player Player;
		private Opponent Opponent;
		
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

			GameInput.AttackerSelected = BattleSystem.OnAttackerSelected;
			GameInput.AttackStopped = BattleSystem.OnAttackStopped;
			GameInput.DefenderSelected = BattleSystem.OnDefenderSelected;
			GameInput.AttackedDirectly = BattleSystem.OnAttackedDirectly;

			Table.PassPlayPressed = GameInput.OnPassPlayPressed;
			Table.EndTurnPressed = GameInput.OnEndTurnPressed;
			Messenger.CallDeferred("SetReady");

			LoadOpponentDeck();
		}

		private async void Execute()
		{
			while(CommandQueue.Count > 0) { await CommandQueue.Dequeue().Execute(Gfx); }
		}

		private void Queue(Commands command, params object[] args)
		{
			Call(command.ToString(), args);
		}
		
		// These 4 Methods (LoadDeck, OpponentLoadDeck, RevealCard & UpdateCard) need to happen immediately..
		// ..rather than be queued otherwise everything else will cause an indexing error.
		private void LoadOpponentDeck()
		{
			for (var i = 0; i < 40; i++)
			{
				var card = CardFactory.Create(0, SetCodes.NullCard);
				AddChild(card); 
				card.OwningPlayer = Opponent;
				card.Controller = Opponent;
				Opponent.Deck.Add(card);
			}
		}
		
		private void LoadDeck(IDictionary<int, SetCodes> deckList)
		{
			foreach (var (key, value) in deckList.Select(p => (p.Key, p.Value)))
			{
				var card = CardFactory.Create(key, value);
				AddChild(card);
				Cards.Add(key, card);
				card.OwningPlayer = Player;
				card.Controller = Player;
				card.MouseOvered = GameInput.OnMousedOverCard;
				card.MouseOveredExit = GameInput.OnMousedOverExitCard;
				Player.StateChanged += card.OnPlayerStateChanged;
				Player.Deck.Add(card);
			}
		}
		
		public void RevealCard(int id, SetCodes setCode, int zoneIds)
		{ 
			var card = CardFactory.Create(id, setCode);
			card.OwningPlayer = Opponent; 
			card.Controller = Opponent; 
			AddChild(card); 
			card.MouseOvered = GameInput.OnMousedOverCard; 
			card.MouseOveredExit = GameInput.OnMousedOverExitCard;
			Cards.Add(id, card);
		}
		
		private void UpdateCard(int id, CardStates state, IList<int> attackTargets, IList<int> targets)
		{
			Cards[id].Update(state, targets, attackTargets);
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
			CommandQueue.Enqueue(new Activate(Opponent, Cards[id]));
		}

		public void SendCardToZone(int cardId, int zoneId)
		{
			CommandQueue.Enqueue(new SendCardToGraveyard(Cards[cardId]));
		}
		
		public void OpponentAttackUnit(int attackerId, int defenderId)
		{
			CommandQueue.Enqueue(new DeclareAttack(Cards[attackerId], Cards[defenderId], BattleSystem));
		}

		public void OpponentAttackDirectly(int attackerId)
		{
			CommandQueue.Enqueue(new DeclareDirectAttack(Player, Cards[attackerId], BattleSystem));
		}
		
		private void BattleUnit(int attackerId, int defenderId, bool isOpponent)
		{
			CommandQueue.Enqueue(new Battle(Cards[attackerId], Cards[defenderId], BattleSystem));
		}

		private void DirectAttack(int attackerId, bool isOpponent)
		{
			CommandQueue.Enqueue(new DirectAttack(GetPlayer(isOpponent), Cards[attackerId], BattleSystem));
		}

		private void LoseLife(int lifeLost, bool isOpponent)
		{
			CommandQueue.Enqueue(new LoseLife(GetPlayer(isOpponent), lifeLost));
		}
		
		private BasePlayer GetPlayer(bool isOpponent)
		{
			return isOpponent ? Opponent : Player as BasePlayer;
		}
	}
}
