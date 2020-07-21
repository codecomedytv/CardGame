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
			Input = new Input(Player, CardCatalog, PlayMat.TargetSelection);
		}
		
		public override void _Ready()
		{
			AddChild(Gfx);
			AddChild(SoundFx);
			AddChild(Messenger);
			AddChild(Input);
			PlayMat.TargetSelection.Connect(nameof(TargetSelection.TargetSelected), Input, nameof(Input.OnTargetSelected));
			CommandQueue.SubscribeTo(Messenger);
			Messenger.SubscribeTo(Input);
			Messenger.Connect(nameof(Messenger.Disqualified), this, nameof(OnDisqualified));
			Messenger.Connect(nameof(Messenger.ExecutedEvents), this, nameof(Execute));
			PlayMat.ActionButton.Connect("pressed", this, nameof(OnActionButtonPressed));
			Messenger.CallDeferred("SetReady");
		}
		
		public async void Execute(States stateAfterExecution)
		{
			await CommandQueue.Execute();
			Player.SetState(stateAfterExecution);
			SetState(stateAfterExecution);
			if (Player.State == States.Targeting)
			{
				GD.Print("Opening Targeting Box");
				PlayMat.TargetSelection.Reveal(Player.Targets);
			}
			// Signal Only Used For Tests (so they can wait for things to be put in place)
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
		}

		private void SetState(States state)
		{
			PlayMat.ActionButton.Text = state switch
			{
				States.Idle => "End Turn",
				States.Active => "Pass Play",
				_ => ""
			};
		}

		private void OnDisqualified()
		{
			PlayMat.DisqualificationNotice.Visible = true;
		}
		
	}
	
}

