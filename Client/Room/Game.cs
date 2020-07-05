using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading.Tasks;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using CardGame.Client.Room.Commands;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Client.Room {

	// ReSharper disable once ClassNeverInstantiated.Global
	public class Game : Control
	{
		[Signal]
		public delegate void StateSet();
		
		private readonly PackedScene PlayMat = (PackedScene) ResourceLoader.Load("res://Client/Room/Game.tscn");
		private readonly Messenger Messenger = new Messenger();
		private readonly CardCatalog CardCatalog = new CardCatalog();
		private Input Input;
		protected Player Player;
		protected Player Opponent;
		private CardViewer CardViewer;
		protected Button PassPriority; // Really Just A Pass Button
		private AnimatedSprite ActionButtonAnimation;
		protected Button EndTurn;
		private Label DisqualificationNotice;
		private List<Command> Commands = new List<Command>();
		
		public override void _Ready()
		{
			var playMat = (Control) PlayMat.Instance();
			playMat.Name = "PlayMat";
			AddChild(playMat, true);
			Player = GetNode<Player>("PlayMat/Player");
			Opponent = GetNode<Player>("PlayMat/Opponent");
			Player.Opponent = Opponent;
			Opponent.Opponent = Player;
			Input = new Input(CardCatalog, Player);
			CardViewer = GetNode<CardViewer>("PlayMat/Background/CardViewer");
			PassPriority = GetNode<Button>("PlayMat/Background/ActionButton");
			ActionButtonAnimation = PassPriority.GetNode<AnimatedSprite>("Glow");
			EndTurn = GetNode<Button>("PlayMat/Background/EndTurn");
			DisqualificationNotice = GetNode<Label>("PlayMat/Disqualified");
			PassPriority.Connect("pressed", this, nameof(OnActionButtonPressed));
			Messenger.Connect(nameof(Messenger.ExecutedEvents), this, nameof(Execute));
			Messenger.Connect(nameof(Messenger.RevealCard), this, nameof(RevealCard));
			Messenger.Connect(nameof(Messenger.UpdateCard), this, nameof(OnCardUpdated));
			Messenger.Connect(nameof(Messenger.Disqualified), this, nameof(OnDisqualified));
			Messenger.Connect(nameof(Messenger.LoadDeck), this, nameof(OnDeckLoaded));
			Messenger.Connect(nameof(Messenger.Draw), this, nameof(OnDrawQueued));
			Messenger.Connect(nameof(Messenger.Deploy), this, nameof(OnDeployQueued));
			Messenger.Connect(nameof(Messenger.SetFaceDown), this, nameof(OnSetFaceDownQueued));
			Messenger.Connect(nameof(Messenger.Activate), this, nameof(OnActivationQueued));
			Messenger.Connect(nameof(Messenger.Trigger), this, nameof(OnTriggeredQueued));
			Messenger.Connect(nameof(Messenger.BattleUnit), this, nameof(OnUnitBattled));
			Messenger.Connect(nameof(Messenger.SendCardToZone), this, nameof(OnCardSentToZone));
			Messenger.Connect(nameof(Messenger.LoseLife), this, nameof(OnLifeLost));
			Input.Connect(nameof(Input.MouseEnteredCard), CardViewer, nameof(CardViewer.OnCardClicked));
			Input.Connect(nameof(Input.Deploy), Messenger, nameof(Messenger.DeclareDeploy));
			Input.Connect(nameof(Input.SetFaceDown), Messenger, nameof(Messenger.DeclareSetFaceDown));
			Input.Connect(nameof(Input.Activate), Messenger, nameof(Messenger.DeclareActivation));
			Input.Connect(nameof(Input.Attack), Messenger, nameof(Messenger.DeclareAttack));
			CardCatalog.Connect(nameof(CardCatalog.CardCreated), Input, nameof(Input.OnCardCreated));
			EndTurn.Connect("pressed", Messenger, nameof(Messenger.DeclareEndTurn));
		}

		public void SetUp()
		{
			AddChild(Messenger, true);
			Messenger.CustomMultiplayer = GetParent().Multiplayer;
			var networkId = CustomMultiplayer.GetNetworkUniqueId();
			Messenger.Id = networkId;
			Messenger.CallDeferred("SetReady");
		}
		
		// A Revealed Card Is Always An Opponent Card
		public void RevealCard(int id, SetCodes setCode, ZoneIds zoneId)
		{
			var card = CardCatalog.Fetch(id, setCode);
			card.Player = Opponent;
			switch (zoneId)
			{
				case ZoneIds.Hand:
				{
					var old = Opponent.Hand.GetChild(0);
					Opponent.Hand.RemoveChild(old);
					Opponent.Hand.AddChild(card);
					Opponent.Sort(Opponent.Hand);
					break;
				}
				case ZoneIds.Support:
				{
					foreach (Card oldCard in Opponent.Support.GetChildren())
					{
						if (!oldCard.IsFaceUp) continue;
						var index = oldCard.GetPositionInParent();
						Opponent.Support.RemoveChild(oldCard);
						Opponent.Support.AddChild(card);
						Opponent.Support.MoveChild(card, index);
						Opponent.Sort(Opponent.Support);
						oldCard.Free();
						return;
					}
					break;
				}
			}
		}

		public void OnCardUpdated(int id, CardStates state, IEnumerable<int> attackTargets, IEnumerable<int> targets)
		{
			var card = CardCatalog.Fetch(id);
			card.State = state;
			card.ValidTargets.Clear();
			card.ValidTargets.AddRange(targets);
			card.ValidAttackTargets.Clear();
			card.ValidAttackTargets.AddRange(attackTargets);
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
			PassPriority.Text = "";
			Messenger.DeclarePassPlay();
		}

		private void SetState(States state)
		{
			PassPriority.Text = "";
			if (state != States.Active) return;
			ActionButtonAnimation.Show();
			ActionButtonAnimation.Play();
			PassPriority.Text = "Pass";
		}

		private void OnDisqualified()
		{
			DisqualificationNotice.Visible = true;
		}

		public void OnDeckLoaded(Dictionary<int, SetCodes> deck)
		{
			foreach (var card in deck.Select(serial => CardCatalog.Fetch(serial.Key, serial.Value)))
			{
				card.Player = Player;
			}
		}
		
		
		private void OnDrawQueued(int id = 0, bool isOpponent = false)
		{
			if(isOpponent) {Opponent.Draw(CardCatalog.Fetch(id));} else {Player.Draw(CardCatalog.Fetch(id));} 
		}

		public void OnDeployQueued(int id, SetCodes setCode, bool isOpponent)
		{
			var card = CardCatalog.Fetch(id); //, setCode);
			if (isOpponent)
			{
				Opponent.Deploy(card);
			}
			else
			{
				Player.Deploy(card);
			}
		}
		
		public void OnSetFaceDownQueued(int id, bool isOpponent)
		{
			if(isOpponent) {Opponent.SetFaceDown(CardCatalog.Fetch(id), true);} else Player.SetFaceDown(CardCatalog.Fetch(id), false);
		}
		
		public void OnActivationQueued(int id, SetCodes setCode, int positionInLink, bool isOpponent)
		{
			var card = CardCatalog.Fetch(id, setCode);
			card.ChainIndex = positionInLink;
			if (isOpponent)
			{
				Opponent.Activate(card);
			}
			else
			{
				Player.Activate(card);
			}
		}

		
		public void OnTriggeredQueued(int id, int positionInLink)
		{
			var card = CardCatalog.Fetch(id);
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
		
		public void OnUnitBattled(int attackerId, int defenderId, bool isOpponent)
		{
			var attacker = CardCatalog.Fetch(attackerId);
			var defender = CardCatalog.Fetch(defenderId);
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
			var card = CardCatalog.Fetch(cardId);
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
			if (isOpponent) { Opponent.LoseLife(lifeLost); } else { Player.LoseLife(lifeLost); }
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

