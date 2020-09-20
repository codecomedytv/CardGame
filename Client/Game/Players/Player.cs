using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using CardGame.Client.Game.Zones.ModelControllers;
using Godot;

namespace CardGame.Client.Game.Players
{
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
		public override Units Units { get; protected set; }
		public override Support Support { get; protected set; }
		public override Hand Hand { get; protected set; }
		public override Graveyard Graveyard { get; protected set; }
		public override Deck Deck { get; protected set; }
		
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
			Units = new Units((UnitsView) GetNode("Units"));
			Support = new Support( (SupportView) GetNode("Support"));
			Hand = new Hand((HandView) GetNode("Hand"));
			Graveyard = new Graveyard((GraveyardView) GetNode("Graveyard"));
			Deck = new Deck((DeckView) GetNode("Deck"));
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
