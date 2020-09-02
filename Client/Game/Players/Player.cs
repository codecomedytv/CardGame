using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Player: Spatial, IPlayer
	{
		[Signal]
		public delegate void StateChanged();

		private Sprite DefendingIcon { get; set; }
		private TextureProgress LifeBar { get; set; }
		private Label LifeCount { get; set; }
		private Label LifeChange { get; set; }
		
		public Units Units { get; set; }
		public Support Support { get; set; }
		public Hand Hand { get; set; }
		public Graveyard Graveyard { get; set; }
		public Deck Deck { get; set; }
		
		private States BackingState;
		public States State
		{
			get => BackingState;
			set => SetState(value);
		}


		private States SetState(States state)
		{
			BackingState = state;
			EmitSignal(nameof(StateChanged), state);
			return state;
		}
		
		// Begin Business Logic
		public bool IsInActive => State != States.Active && State != States.Idle;
		public bool Attacking = false;
		public Card CardInUse = null;
		public bool IsChoosingAttackTarget => Attacking && State == States.Idle;
		// End Business Logic
		
		// Begin Animation Logic
		public override void _Ready()
		{
			Units = (Units) GetNode("Units");
			Support = (Support) GetNode("Support");
			Hand = (Hand) GetNode("Hand");
			Graveyard = (Graveyard) GetNode("Graveyard");
			Deck = (Deck) GetNode("Deck");
			LifeBar = (TextureProgress) GetNode("Life/Bar");
			LifeCount = (Label) GetNode("Life/Count");
			LifeChange = (Label) GetNode("Life/Change");
			DefendingIcon = (Sprite) GetNode("Defending");
		}
		
		public void LoseLife(int lifeLost, Tween gfx)
		{
			var newLife = GD.Str(LifeCount.Text.ToInt() - lifeLost);
			var percentage = 100 - (int) ((lifeLost / 8000F) * 100);
			LifeChange.Text = $"- {lifeLost}";
			gfx.InterpolateCallback(LifeChange, 0.1F, "set_visible", true);
			gfx.InterpolateCallback(LifeCount, 0.3F, "set_text", newLife);
			gfx.InterpolateProperty(LifeBar, "value", (int) LifeBar.Value, percentage, 0.3F);
			gfx.InterpolateCallback(LifeChange, 0.5F, "set_visible", false);
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
