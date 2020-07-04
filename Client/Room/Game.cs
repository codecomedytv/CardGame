using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using Godot;
using Godot.Collections;
using Card = CardGame.Server.Game.Cards.Card;

namespace CardGame.Client.Room {

	// ReSharper disable once ClassNeverInstantiated.Global
	public class Game : Control
	{
		[Signal] 
		public delegate void EndedTurn();

		[Signal]
		public delegate void StateSet();
		
		private readonly PackedScene PlayMat = (PackedScene) ResourceLoader.Load("res://Client/Room/Game.tscn");
		private readonly Messenger Messenger = new Messenger();
		private readonly CardCatalog CardCatalog = new CardCatalog();
		protected Player Player;
		protected Player Opponent;
		private CardViewer CardViewer;
		private Button ActionButton;
		private AnimatedSprite ActionButtonAnimation;
		private Button EndTurn;
		private Label DisqualificationNotice;
		
		public override void _Ready()
		{
			var playMat = (Control) PlayMat.Instance();
			playMat.Name = "PlayMat";
			AddChild(playMat, true);
			Player = GetNode<Player>("PlayMat/Player");
			Opponent = GetNode<Player>("PlayMat/Opponent");
			Player.Opponent = Opponent;
			Opponent.Opponent = Player;
			CardCatalog.User = Player;
			CardViewer = GetNode<CardViewer>("PlayMat/Background/CardViewer");
			ActionButton = GetNode<Button>("PlayMat/Background/ActionButton");
			ActionButtonAnimation = ActionButton.GetNode<AnimatedSprite>("Glow");
			EndTurn = GetNode<Button>("PlayMat/Background/EndTurn");
			DisqualificationNotice = GetNode<Label>("PlayMat/Disqualified");
			ActionButton.Connect("pressed", this, nameof(OnActionButtonPressed));
			Messenger.Connect(nameof(Messenger.ExecutedEvents), this, nameof(Execute));
			Messenger.Connect(nameof(Messenger.Disqualified), this, nameof(OnDisqualified));
			Messenger.Connect(nameof(Messenger.DeckLoaded), this, nameof(OnDeckLoaded));
			Messenger.Connect(nameof(Messenger.CardStateSet), this, nameof(OnCardStateSet));
			Messenger.Connect(nameof(Messenger.DrawQueued), this, nameof(OnDrawQueued));
			Messenger.Connect(nameof(Messenger.DeployQueued), this, nameof(OnDeployQueued));
			Messenger.Connect(nameof(Messenger.SetFaceDownQueued), this, nameof(OnSetFaceDownQueued));
			Messenger.Connect(nameof(Messenger.ActivationQueued), this, nameof(OnActivationQueued));
			Messenger.Connect(nameof(Messenger.TriggerQueued), this, nameof(OnTriggeredQueued));
			Messenger.Connect(nameof(Messenger.ValidTargetsSet), this, nameof(OnValidTargetsSet));
			Messenger.Connect(nameof(Messenger.ValidAttackTargetsSet), this, nameof(OnValidAttackTargetsSet));
			Messenger.Connect(nameof(Messenger.UnitBattled), this, nameof(OnUnitBattled));
			Messenger.Connect(nameof(Messenger.CardSentToZone), this, nameof(OnCardSentToZone));
			Messenger.Connect(nameof(Messenger.LifeLost), this, nameof(OnLifeLost));
			CardCatalog.Connect(nameof(CardCatalog.MouseEnteredCard), CardViewer, nameof(CardViewer.OnCardClicked));
			CardCatalog.Connect(nameof(CardCatalog.Deploy), Messenger, nameof(Messenger.Deploy));
			CardCatalog.Connect(nameof(CardCatalog.SetFaceDown), Messenger, nameof(Messenger.SetFaceDown));
			CardCatalog.Connect(nameof(CardCatalog.Activate), Messenger, nameof(Messenger.Activate));
			CardCatalog.Connect(nameof(CardCatalog.Attack), Messenger, nameof(Messenger.Attack));
			Connect(nameof(StateSet), CardCatalog, nameof(CardCatalog.OnStateSet));
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

		private async void Execute(States stateAfterExecution)
		{
			await Task.WhenAll(new List<Task> {Player.Execute(), Opponent.Execute()});
			Player.SetState(stateAfterExecution);
			Player.Reset();
			Opponent.Reset();
			SetState(stateAfterExecution);
			EmitSignal(nameof(StateSet));
		}

		protected void OnActionButtonPressed()
		{
			if (Player.State != States.Active)
			{
				return;
			}

			ActionButtonAnimation.Stop();
			ActionButtonAnimation.Hide();
			ActionButtonAnimation.Frame = 0;
			ActionButton.Text = "";
			Messenger.PassPriority();
		}

		private void SetState(States state)
		{
			ActionButton.Text = "";
			if (state != States.Active) return;
			ActionButtonAnimation.Show();
			ActionButtonAnimation.Play();
			ActionButton.Text = "Pass";
		}

		private void OnDisqualified()
		{
			DisqualificationNotice.Visible = true;
		}

		public void OnDeckLoaded(System.Collections.Generic.Dictionary<int, SetCodes> deck)
		{
			foreach (var card in deck.Select(serial => CheckOut.Fetch(serial.Key, serial.Value)))
			{
				CardCatalog[card.Id] = card;
				card.Player = Player;
				// Set Zone
			}
		}
		
		public void OnCardStateSet(int id, CardStates state)
		{
			CardCatalog[id].State = state;
		}

		private void OnDrawQueued(int id, SetCodes setCode)
		{
			var card = CardCatalog[id];
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
			Player.Deploy(card, false);
		}

		public void OnDeployQueued(int id, SetCodes setCode)
		{
			var card = CheckOut.Fetch(id, setCode);
			card.Player = Opponent;
			CardCatalog[id] = card;
			Opponent.Deploy(card, true);
		}

		public void OnSetFaceDownQueued(int id)
		{
			var card = CardCatalog[id];
			Player.SetFaceDown(card);
		}

		public void OnSetFaceDownQueued()
		{
			Opponent.SetFaceDown();
		}

		public void OnActivationQueued(int id, int positionInLink)
		{
			var card = CardCatalog[id];
			card.ChainIndex = positionInLink;
			Player.Activate(card);
		}

		public void OnActivationQueued(int id, SetCodes setCode, int positionInLink)
		{
			var card = CheckOut.Fetch(id, setCode);
			card.Player = Opponent;
			CardCatalog[id] = card;
			card.ChainIndex = positionInLink;
			Opponent.Activate(card, true);
		}

		public void OnTriggeredQueued(int id, int positionInLink)
		{
			var card = CardCatalog[id];
			card.ChainIndex = positionInLink;
			if (card.Player == Player)
			{
				Player.Trigger(card);
			}
			else
			{
				Opponent.Trigger(card);
			}
		}

		private void OnValidTargetsSet(int id, IEnumerable<int> validTargets)
		{
			var card = CardCatalog[id];
			card.ValidTargets.Clear();
			card.ValidTargets.AddRange(validTargets);
		}

		public void OnValidAttackTargetsSet(int id, IEnumerable<int> validAttackTargets)
		{
			var card = CardCatalog[id];
			card.ValidAttackTargets.Clear();
			card.ValidAttackTargets.AddRange(validAttackTargets);
		}
		
		public void OnUnitBattled(int attackerId, int defenderId, bool isOpponent)
		{
			var attacker = CardCatalog[attackerId];
			var defender = CardCatalog[defenderId];
			if (isOpponent)
			{
				Opponent.Battle(attacker, defender, true);
			}
			else
			{
				Player.Battle(attacker, defender, false);
			}
		}

		public void OnCardSentToZone(int cardId, ZoneIds zoneId)
		{
			var card = CardCatalog[cardId];
			if (card.Player == Player)
			{
				Player.SendCardToZone(card, zoneId);
			}
			else
			{
				Opponent.SendCardToZone(card, zoneId);
			}
		}

		public void OnLifeLost(int lifeLost, bool isOpponent)
		{
			if (isOpponent)
			{
				Opponent.LoseLife(lifeLost);
			}
			else
			{
				Player.LoseLife(lifeLost);
			}
		}

		protected void OnEndTurn()
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

