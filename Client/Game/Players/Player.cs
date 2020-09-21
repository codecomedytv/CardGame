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

		public override int Health
		{
			get => HealthBar.Value;
			set => SetHealth(value);
		}
		public override Zone Units { get; protected set; }
		public override Zone Support { get; protected set; }
		public override Zone Hand { get; protected set; }
		public override Zone Graveyard { get; protected set; }
		public override Zone Deck { get; protected set; }
		
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
		
		public override void _Ready()
		{
			Units = new Zone((Spatial) GetNode("Units"));
			Support = new Zone( (Spatial) GetNode("Support"));
			Hand = new Zone((Spatial) GetNode("Hand"));
			Graveyard = new Zone((Spatial) GetNode("Graveyard"));
			Deck = new Zone((Spatial) GetNode("Deck"));
			HealthBar = (HealthBar) GetNode("HUD/Health");
			EnergyIcon = (Sprite) GetNode("HUD/EnergyIcon");
		}

		private int SetHealth(int health)
		{
			HealthBar.OnHealthChanged(Health - health);
			return health;
		}
	}
}
