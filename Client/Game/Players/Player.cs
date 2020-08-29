using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Player: Spatial, IPlayer
	{
		[Signal]
		private delegate void StateChanged();

		private Sprite DefendingIcon { get; set; }
		private TextureProgress LifeBar { get; set; }
		private Label LifeCount { get; set; }
		private Label LifeChange { get; set; }
		
		private Units Units;
		private Support Support;
		private Hand Hand;
		private Graveyard Graveyard;
		private Deck Deck;
		private Tween Gfx;
		private AudioStreamPlayer Sfx;
		
		private States BackingState;
		public States State
		{
			get => BackingState;
			set => SetState(value);
		}


		public States SetState(States state)
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
			Gfx = (Tween) GetNode("GFX");
			Sfx = (AudioStreamPlayer) GetNode("SFX");
			LifeBar = (TextureProgress) GetNode("Life/Bar");
			LifeCount = (Label) GetNode("Life/Count");
			LifeChange = (Label) GetNode("Life/Change");
			DefendingIcon = (Sprite) GetNode("Defending");
		}
		
		public void LoadDeck(IEnumerable<Card> deck)
		{
			foreach (var card in deck)
			{
				card.Player = this;
				Connect(nameof(StateChanged), card, nameof(Card.OnPlayerStateChanged));
				AddCardToDeck(card);
			}
		}
		
		public void AddCardToDeck(Card card)
		{
			Deck.Add(card);
		}
		

		public void DisplayName(string name)
		{
			throw new System.NotImplementedException();
		}

		public void DisplayHealth(int health)
		{
			throw new System.NotImplementedException();
		}

		public Command Draw(Card card)
		{
			Tween Command()
			{
				card.Visible = false;
				Deck.Add(card);
				var globalPosition = card.Translation;
				Deck.Remove(card);
				Hand.Add(card);
				var globalDestination = card.Translation;
				var rotation = new Vector3(-25, 180, 0);
				
				// Wrap In GFX Class
				Gfx.InterpolateProperty(card, nameof(Visible), false, true, 0.1F);
				Gfx.InterpolateProperty(card, nameof(Translation), globalPosition, globalDestination, 0.1F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), card.Rotation, rotation, 0.1F);
				return Gfx;
			}

			return Command;
		}
		
		public Command Discard(Card card)
		{
			throw new System.NotImplementedException();
		}

		public Command Deploy(Card card)
		{
			Tween Command()
			{
				var origin = card.Translation;
				var destination = Units.NextSlot() + new Vector3(0, 0, 0.05F);

				Hand.Remove(card);
				Units.Add(card);

				Gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 180, 0), 0.1F);
				Gfx.InterpolateCallback(Hand, 0.2F, nameof(Hand.Sort));
				return Gfx;
			}

			return Command;
		}
		
		public Command SetFaceDown(Card card)
		{
			Tween Command()
			{
				var origin = card.Translation;
				var destination = Support.NextSlot() + new Vector3(0, 0, 0.05F);

				Hand.Remove(card);
				Support.Add(card);

				Gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 0, 0), 0.1F);
				Gfx.InterpolateCallback(Hand, 0.2F, nameof(Hand.Sort));
				return Gfx;
			}
			
			return Command;
		}

		public Command Activate(Card card)
		{
			GD.Print("Activate");

			Tween Command()
			{
				return Gfx;
			}

			return Command;
		}
		
		public Command SendCardToGraveyard(Card card)
		{
			Tween Command()
			{
				if (Units.Contains(card))
				{
					Units.Remove(card);
				}
				else if(Support.Contains(card))
				{
					Support.Remove(card);
				}
				
				Graveyard.Add(card);
				
				var origin = card.Translation;
				var destination = Graveyard.GlobalTransform.origin + new Vector3(0, 0, 0.1F);

				Gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				return Gfx;
			}
			
			return Command;
		}

		public Command Attack(Card attacker, Card defender)
		{
			throw new System.NotImplementedException();
		}
		

		public Command AttackDirectly(Card attacker)
		{
			Tween Command()
			{
				var destination = new Vector3(2.5F, 9F, attacker.Translation.z);

				Gfx.InterpolateProperty(attacker, nameof(Translation), attacker.Translation, destination, 0.1F);
				Gfx.InterpolateProperty(attacker, nameof(Translation), destination, attacker.Translation, 0.1F,
					Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
				
				Gfx.InterpolateCallback(attacker.AttackingIcon, 0.2F, "set_visible", false);
				return Gfx;
			}

			return Command;
		}

		public Command LoseLife(int lifeLost)
		{
			var newLife = GD.Str(LifeCount.Text.ToInt() - lifeLost);
			var percentage = 100 - (int) ((lifeLost / 8000F) * 100);
			
			Tween Command()
			{
				LifeChange.Text = $"- {lifeLost}";
				Gfx.InterpolateCallback(LifeChange, 0.1F, "set_visible", true);
				Gfx.InterpolateCallback(LifeCount, 0.3F, "set_text", newLife);
				
				Gfx.InterpolateProperty(LifeBar, "value", (int) LifeBar.Value, percentage, 0.3F);
				Gfx.InterpolateCallback(LifeChange, 0.5F, "set_visible", false);
				return Gfx;
			}

			return Command;
		}
		
		public Command Battle(Card attacker, Card defender)
		{
			Tween Command()
			{
				var attackerDestination = new Vector3(2.5F, 0.5F, attacker.Translation.z);
				var defenderDestination = new Vector3(2.5F, 1.75F, defender.Translation.z);

				Gfx.InterpolateProperty(attacker, nameof(Translation), attacker.Translation, attackerDestination, 0.1F);
				Gfx.InterpolateProperty(defender, nameof(Translation), defender.Translation, defenderDestination, 0.1F);
				// Insert Sound
				Gfx.InterpolateProperty(attacker, nameof(Translation), attackerDestination, attacker.Translation, 0.1F, 
					Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
				Gfx.InterpolateProperty(defender, nameof(Translation), defenderDestination, defender.Translation, 0.1F,
					Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);

				Gfx.InterpolateCallback(this, 0.4F, nameof(ClearBattle), attacker, defender);
				
				return Gfx;
			}

			return Command;
		}

		private void ClearBattle(Card attacker, Card defender)
		{
			attacker.AttackingIcon.Visible = false;
			defender.DefendingIcon.Visible = false;
		}

		public Command GetAttackedDirectly(Card attacker)
		{
			// Declaration, not battle
			Tween Command()
			{
				Gfx.InterpolateCallback(attacker, 0.1F, nameof(Card.Attack));
				Gfx.InterpolateCallback(this, 0.1F, nameof(Defend));
				return Gfx;
			}
			
			return Command;
		}

		private void Defend()
		{
			DefendingIcon.Visible = true;
		}
	}
}
