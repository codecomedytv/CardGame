using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Client.Room.Commands;
using CardGame.Client.Room.View;
using Godot;

namespace CardGame.Client.Room {

	// ReSharper disable once ClassNeverInstantiated.Global
	public class Game : Control
	{
		[Signal]
		public delegate void StateSet();

		protected readonly PlayMat PlayMat;
		private readonly CommandQueue CommandQueue;
		private readonly CardCatalog CardCatalog;
		private readonly Messenger Messenger;
		protected readonly Input Input;
		private readonly Tween Gfx;
		private readonly SoundFx SoundFx;
		protected readonly Player Opponent;
		protected readonly Player Player;

		protected Game(PlayMat playMat, string gameId)
		{
			Name = gameId;
			PlayMat = playMat;
			Gfx = new Tween();
			SoundFx = new SoundFx();
			Player = new Player(playMat.Player);
			Opponent = new Player(playMat.Opponent);
			Player.Opponent = Opponent;
			Opponent.Opponent = Player;
			CardCatalog = new CardCatalog();
			Messenger = new Messenger();
			CommandQueue = new CommandQueue(CardCatalog, Player, Opponent, Gfx, SoundFx, PlayMat.WinLoseNotice);
			Input = new Input(Player, CardCatalog);
		}
		
		public override void _Ready()
		{
			AddChild(Gfx);
			AddChild(SoundFx);
			AddChild(Messenger);
			AddChild(Input);
			CommandQueue.SubscribeTo(Messenger);
			Messenger.SubscribeTo(Input);
			Messenger.Connect(nameof(Messenger.Disqualified), this, nameof(OnDisqualified));
			Messenger.Connect(nameof(Messenger.ExecutedEvents), this, nameof(Execute));
			//CardCatalog.Connect(nameof(CardCatalog.CardCreated), Input, nameof(Input.OnCardCreated));
			PlayMat.ActionButton.Connect("pressed", this, nameof(OnActionButtonPressed));
			//PlayMat.PassPriority.Connect("pressed", this, nameof(OnActionButtonPressed));
			//PlayMat.EndTurn.Connect("pressed", Messenger, nameof(Messenger.DeclareEndTurn));
			
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
			// ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
			switch (Player.State)
			{
				case States.Idle when PlayMat.ActionButton.Text == "End Turn":
					Messenger.DeclareEndTurn();
					return;
				case States.Active when PlayMat.ActionButton.Text == "Pass Play":
					Messenger.DeclarePassPlay();
					return;
			}

			// PlayMat.ActionButtonAnimation.Stop();
			// PlayMat.ActionButtonAnimation.Hide();
			// PlayMat.ActionButtonAnimation.Frame = 0;
			// PlayMat.PassPriority.Text = "";
		}

		private void SetState(States state)
		{
			//PlayMat.PassPriority.Text = "";
			PlayMat.ActionButton.Text = state switch
			{
				States.Idle => "End Turn",
				States.Active => "Pass Play",
				_ => ""
			};
			//if (state != States.Active) return;
			//PlayMat.ActionButtonAnimation.Show();
			//PlayMat.ActionButtonAnimation.Play();
			//PlayMat.PassPriority.Text = "Pass";
		}

		private void OnDisqualified()
		{
			PlayMat.DisqualificationNotice.Visible = true;
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

