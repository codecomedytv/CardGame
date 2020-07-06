using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Client.Library.Cards;
using CardGame.Client.Room.Commands;
using CardGame.Client.Room.View;
using Godot;

namespace CardGame.Client.Room {

	// ReSharper disable once ClassNeverInstantiated.Global
	public class Game : Control
	{
		[Signal]
		public delegate void StateSet();

		private readonly PlayMat PlayMat;
		private readonly CommandQueue CommandQueue;
		private readonly CardCatalog CardCatalog;
		private readonly Messenger Messenger;
		private readonly Input Input;
		private readonly Tween Gfx;
		protected readonly Player Opponent;
		protected readonly Player Player;
		
		private Label DisqualificationNotice;
		private AnimatedSprite ActionButtonAnimation;
		private Button PassPriority; // Really Just A Pass Button
		private CardViewer CardViewer;
		protected Button EndTurn;

		public Game(PlayMat playMat)
		{
			PlayMat = playMat;
			Gfx = new Tween();
			Player = new Player(playMat.Player);
			Opponent = new Player(playMat.Opponent);
			CardCatalog = new CardCatalog();
			Messenger = new Messenger();
			CommandQueue = new CommandQueue(CardCatalog, Player, Opponent, Gfx);
			Input = new Input(CardCatalog, Player);
			CommandQueue.SubscribeTo(Messenger);
			Messenger.SubscribeTo(Input);
			Messenger.Connect(nameof(Messenger.Disqualified), this, nameof(OnDisqualified));
			Messenger.Connect(nameof(Messenger.ExecutedEvents), this, nameof(Execute));
			CardCatalog.Connect(nameof(CardCatalog.CardCreated), Input, nameof(Input.OnCardCreated));
		}
		
		public override void _Ready()
		{
			AddChild(Gfx);
			//Player.Initialize(PlayMat.GetNode<Control>("Player"));
			//Opponent.Initialize(PlayMat.GetNode<Control>("Opponent"));
			CardViewer = PlayMat.GetNode<CardViewer>("Background/CardViewer");
			Input.Connect(nameof(Input.MouseEnteredCard), CardViewer, nameof(CardViewer.OnCardClicked));
			PassPriority = PlayMat.GetNode<Button>("Background/ActionButton");
			ActionButtonAnimation = PassPriority.GetNode<AnimatedSprite>("Glow");
			EndTurn = PlayMat.GetNode<Button>("Background/EndTurn");
			DisqualificationNotice = PlayMat.GetNode<Label>("Disqualified");
			
			PassPriority.Connect("pressed", this, nameof(OnActionButtonPressed));
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

		public async void Execute(States stateAfterExecution)
		{
			await CommandQueue.Execute();
			Player.SetState(stateAfterExecution);
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

