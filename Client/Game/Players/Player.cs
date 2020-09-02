using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Player: Spatial, IPlayer
	{
		public event Action<States> StateChanged;

		private Sprite DefendingIcon { get; set; }
		
		private States BackingState;
		public States State { get => BackingState; set => SetState(value); }
		private HealthBar HealthBar { get; set; }
		public int Health { get => HealthBar.Value; set => SetHealth(value); }
		public Units Units { get; private set; }
		public Support Support { get; private set; }
		public Hand Hand { get; private set; }
		public Graveyard Graveyard { get; private set; }
		public Deck Deck { get; private set; }
		
		
		
		private States SetState(States state)
		{
			BackingState = state;
			GD.Print("Setting State Change");
			StateChanged?.Invoke(state);
			return state;
		}
		
		public bool IsInActive => State != States.Active && State != States.Idle;
		public bool Attacking = false;
		public Card CardInUse = null;
		public bool IsChoosingAttackTarget => Attacking && State == States.Idle;
		
		public override void _Ready()
		{
			Units = (Units) GetNode("Units");
			Support = (Support) GetNode("Support");
			Hand = (Hand) GetNode("Hand");
			Graveyard = (Graveyard) GetNode("Graveyard");
			Deck = (Deck) GetNode("Deck");
			DefendingIcon = (Sprite) GetNode("Defending");
			HealthBar = (HealthBar) GetNode("Health");
		}

		private int SetHealth(int health)
		{
			HealthBar.OnHealthChanged(Health - health);
			return health;
		}
		
		public void StopDefending()
		{
			DefendingIcon.Visible = false;
		}
		
		public void Defend()
		{
			DefendingIcon.Visible = true;
		}
	}
}
