using System.Threading.Tasks;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using CardGame.Client.Player;
using Godot;

namespace CardGame.Client.Room {

	// ReSharper disable once ClassNeverInstantiated.Global
	public class Game : Control
	{
		[Signal]
		public delegate void EndedTurn();
		
		private readonly Messenger Messenger = new Messenger();
		private readonly CardCatalog CardCatalog = new CardCatalog();
		private readonly Tween Gfx = new Tween();
		private Controller Player;
		private Controller Opponent;
		private CardViewer CardViewer;
		private Button EndTurn;
		public override void _Ready()
		{
			AddChild(Gfx);
			Player = new Controller(GetNode<View>("Player"), true);
			Opponent = new Controller(GetNode<View>("Opponent"), false);
			CardViewer = GetNode<CardViewer>("Background/CardViewer");
			EndTurn = GetNode<Button>("Background/EndTurn");
			Messenger.Connect(nameof(Messenger.CardStateSet), this, nameof(OnCardStateSet));
			Messenger.Connect(nameof(Messenger.StateQueued), this, nameof(OnStateQueued));
			Messenger.Connect(nameof(Messenger.DrawQueued), this, nameof(OnDrawQueued));
			Messenger.Connect(nameof(Messenger.DeployQueued), this, nameof(OnDeployQueued));
			Messenger.Connect(nameof(Messenger.ExecutedEvents), this, nameof(Execute));
			CardCatalog.Connect(nameof(CardCatalog.CardClicked), CardViewer, nameof(CardViewer.OnCardClicked));
			CardCatalog.Connect(nameof(CardCatalog.Deploy), Messenger, nameof(Messenger.Deploy));
			CardCatalog.Connect(nameof(CardCatalog.SetFaceDown), Messenger, nameof(Messenger.SetFaceDown));
			EndTurn.Connect("pressed", this, nameof(OnEndTurn));
			Connect(nameof(EndedTurn), Messenger, nameof(Messenger.EndTurn));
		}

		public void SetUp()
		{
			AddChild(Messenger, true);
			Messenger.CustomMultiplayer = GetParent().Multiplayer;
			var networkId = CustomMultiplayer.GetNetworkUniqueId();
			Messenger.Id = networkId;
			Messenger.CallDeferred("SetReady");
		}

		private void Execute()
		{
			Player.Execute();
			Opponent.Execute();
		}
		
		public void OnStateQueued(States state)
		{
			Player.SetState(state);
		}

		public void OnCardStateSet(int id, CardStates state)
		{
			CardCatalog[id].State = state;
		}

		private void OnDrawQueued(int id, SetCodes setCode)
		{
			var card = CheckOut.Fetch(id, setCode);
			CardCatalog[id] = card;
			Player.Draw(card);
		}

		private void OnDrawQueued()
		{
			var card = CheckOut.Fetch(0, SetCodes.NullCard);
			Opponent.Draw(card);
		}

		public void OnDeployQueued(int id)
		{
			var card = CardCatalog[id];
			Player.Deploy(card);
		}

		public void OnDeployQueued(int id, SetCodes setCode)
		{
			var card = CheckOut.Fetch(id, setCode);
			CardCatalog[id] = card;
			Opponent.Deploy(card, true);
		}

		private void OnEndTurn()
		{
			EmitSignal(nameof(EndedTurn));
		}
		
		public void _Connect(Godot.Object emitter, string signal, Godot.Object receiver, string method)
		{
			var err = emitter.Connect(signal, receiver, method);
			if (err != Error.Ok)
			{
				GD.PushWarning(err.ToString());
			}
		}

	}
	
}

