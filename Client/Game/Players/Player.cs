using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Game.Players
{
	[UsedImplicitly]
	public class Player: BasePlayer
	{
		public event Action<States> StateChanged;
		private Sprite EnergyIcon { get; set; }
		
		private States BackingState;
		public States State { get => BackingState; set => SetState(value); }
		private HealthBar HealthBar { get; set; }
		
		// Should really be a separate class
		private States SetState(States state)
		{
			if (state == States.Idle || state == States.Active)
			{
				EnergyIcon.Modulate = Colors.Gold;
			}
			else
			{
				EnergyIcon.Modulate = Colors.Black;
			}
			BackingState = state;
			StateChanged?.Invoke(state);
			return state;
		}
		
		public bool IsInActive => State != States.Active && State != States.Idle;
		public bool Attacking = false;
		public Card CardInUse = null;
		public bool IsChoosingAttackTarget => Attacking && State == States.Idle;

		private Player() { }
		public Player(PlayerView view): base(view)
		{
			EnergyIcon = (Sprite) View.GetNode("HUD/EnergyIcon");
		}
	}
}
