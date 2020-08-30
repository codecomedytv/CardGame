using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Opponent: Spatial, IPlayer
	{
		private Sprite DefendingIcon { get; set; }
		private TextureProgress LifeBar { get; set; }
		private Label LifeCount { get; set; }
		private Label LifeChange { get; set; }
		public Units Units { get; set; }
		public Support Support { get; set; }
		public Hand Hand { get; set; }
		public Graveyard Graveyard { get; set; }
		public Deck Deck { get; set; }
		private AudioStreamPlayer Sfx;

		public override void _Ready()
		{
			Units = (Units) GetNode("Units");
			Support = (Support) GetNode("Support");
			Hand = (Hand) GetNode("Hand");
			Graveyard = (Graveyard) GetNode("Graveyard");
			Deck = (Deck) GetNode("Deck");
			Sfx = (AudioStreamPlayer) GetNode("SFX");
			LifeBar = (TextureProgress) GetNode("Life/Bar");
			LifeCount = (Label) GetNode("Life/Count");
			LifeChange = (Label) GetNode("Life/Change");
			DefendingIcon = (Sprite) GetNode("Defending");
		}
		
		public void AddCardToDeck(Card card)
		{
			Deck.Add(card);
		}
		
		public void LoadDeck(IEnumerable<Card> deck)
		{
			foreach (var card in deck)
			{
				AddCardToDeck(card);
			}
		}
	
		public Command SetFaceDown(Card ignoredCard)
		{
			Tween Command(Tween gfx)
			{
				var card = Hand[0];
				var origin = card.Translation;
				var destination = Support.NextSlot() + new Vector3(0, 0, 0.05F);

				Hand.Remove(card);
				Support.Add(card);

				gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				gfx.InterpolateProperty(card, nameof(RotationDegrees), new Vector3(-25, 0, 0), new Vector3(0, 0, 0),
					0.1F);
				gfx.InterpolateCallback(Hand, 0.2F, nameof(Hand.Sort));
				return gfx;
			}

			return Command;
		}

		public Command Activate(Card card)
		{
			Tween Command(Tween gfx)
			{
				var fakeCard = Support[0];
				Support.Remove(fakeCard);
				Support.Add(card);
				card.Translation = fakeCard.Translation;
				fakeCard.Free();

				gfx.InterpolateProperty(card, nameof(RotationDegrees), new Vector3(0, 0, 0), new Vector3(0, 180, 0),
					0.1F);

				return gfx;
			}

			return Command;
		}

		public Command SendCardToGraveyard(Card card)
		{
			Tween Command(Tween gfx)
			{
				if (Units.Contains(card))
				{
					Units.Remove(card);
				}
				else if (Support.Contains(card))
				{
					Support.Remove(card);
				}

				Graveyard.Add(card);

				var origin = card.Translation;
				var destination = Graveyard.GlobalTransform.origin + new Vector3(0, 0, 0.05F);

				gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				return gfx;
			}

			return Command;
		}

		public Command Attack(Card attacker, Card defender)
		{
			Tween Command(Tween gfx)
			{
				gfx.InterpolateCallback(attacker, 0.1F, nameof(Card.Attack));
				gfx.InterpolateCallback(defender, 0.1F, nameof(Card.Defend));
				return gfx;
			}

			return Command;
		}

		public Command AttackDirectly(Card attacker)
		{
			Tween Command(Tween gfx)
			{
				var destination = new Vector3(2.5F, -2.95F, 1);

				gfx.InterpolateProperty(attacker, nameof(Translation), attacker.Translation, destination, 0.1F);
				gfx.InterpolateProperty(attacker, nameof(Translation), destination, attacker.Translation, 0.1F,
					Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
				gfx.InterpolateCallback(attacker.AttackingIcon, 0.2F, "set_visible", false);
				// gfx.InterpolateCallback(Player, 0.3F, nameof(IPlayer.ClearDirectAttackingDefense));
				return gfx;
			}

			return Command;
		}

		public Command Battle(Card attacker, Card defender)
		{
			Tween Command(Tween gfx)
			{
				var attackerDestination = new Vector3(2.5F, 1.75F, attacker.Translation.z);
				var defenderDestination = new Vector3(2.5F, 0.5F, defender.Translation.z);

				gfx.InterpolateProperty(attacker, nameof(Translation), attacker.Translation, attackerDestination,
					0.1F);
				gfx.InterpolateProperty(defender, nameof(Translation), defender.Translation, defenderDestination,
					0.1F);
				// Sound // Extra Here
				gfx.InterpolateProperty(attacker, nameof(Translation), attackerDestination, attacker.Translation,
					0.1F,
					Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
				gfx.InterpolateProperty(defender, nameof(Translation), defenderDestination, defender.Translation,
					0.1F,
					Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);

				gfx.InterpolateCallback(this, 0.4F, nameof(ClearBattle), attacker, defender);

				return gfx;
			}

			return Command;
		}

		public Command LoseLife(int lifeLost)
		{
			var newLife = GD.Str(LifeCount.Text.ToInt() - lifeLost);
			var percentage = 100 - (int) ((lifeLost / 8000F) * 100);

			Tween Command(Tween gfx)
			{
				LifeChange.Text = $"- {lifeLost}";
				gfx.InterpolateCallback(LifeChange, 0.1F, "set_visible", true);
				gfx.InterpolateCallback(LifeCount, 0.3F, "set_text", newLife);
				gfx.InterpolateProperty(LifeBar, "value", (int) LifeBar.Value, percentage, 0.3F);
				gfx.InterpolateCallback(LifeChange, 0.5F, "set_visible", false);
				return gfx;
			}

			return Command;
		}

		public void ClearBattle(Card attacker, Card defender)
		{
			attacker.AttackingIcon.Visible = false;
			defender.DefendingIcon.Visible = false;
		}

		public void Defend()
		{
			DefendingIcon.Visible = true;
		}

		public void ClearDirectAttackingDefense()
		{
			DefendingIcon.Visible = false;
		}
	}
}
