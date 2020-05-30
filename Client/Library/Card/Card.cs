using System;
using CardGame.Client.Match;
using CardGame.Server;
using Godot;
using Array = Godot.Collections.Array;
using Environment = Godot.Environment;
using Object = Godot.Object;
using Player = CardGame.Client.Match.Player;

namespace CardGame.Client.Library.Card
{
	public class Card : Control
	{
		public enum Zones
		{
			Deck,
			Discard,
			Hand,
			Unit,
			Support
		};

		public int Id = 0;
		public string Title;
		public string Illustration;
		public int Attack = 0;
		public int Defense = 0;
		public string Effect;
		public CardTypes CardType;
		public bool IsReady = false;
		public bool Blank = false;
		public bool Legal = false;
		public Zones Zone = Zones.Deck;
		public bool UnderPlayersControl = false;
		public string Type = "None";
		public Array ValidTargets = new Array();
		public Array SelectedTargets = new Array();
		public bool Activated = false;
		public bool CanBeDeployed = false;
		public bool CanBeSet = false;
		public bool CanBeActivated = false;
		public bool CanAttack = false;
		public Player Player; // Client Player doesn't exist yet

		// Set by Game
		public Interact Interact;

		// Visual Onready Vars
		public AnimatedSprite LegalPlay;
		public Label Link;
		public Label Identifier;
		public Label TitleLabel;
		public Label AttackLabel;
		public Label DefenseLabel;
		public Sprite Frame;
		public Sprite Art;
		public TextureRect Back;
		public Panel Rest;
		public AudioStreamPlayer Sound;
		public AnimatedSprite ValidTarget;
		public AnimatedSprite SelectedTarget;
		public TextureRect Combat;

		public override void _Ready()
		{
			LegalPlay = GetNode("Frame/LegalPlay") as AnimatedSprite;
			Link = GetNode("Link") as Label;
			Identifier = GetNode("ID") as Label;
			TitleLabel = GetNode("Title") as Label;
			AttackLabel = GetNode("Battle/Attack") as Label;
			DefenseLabel = GetNode("Battle/Defense") as Label;
			Frame = GetNode("Frame") as Sprite;
			Art = GetNode("Frame/Illustration") as Sprite;
			Back = GetNode("Back") as TextureRect;
			Rest = GetNode("Rest") as Panel;
			Sound = GetNode("AudioStreamPlayer") as AudioStreamPlayer;
			ValidTarget = GetNode("Frame/ValidTarget") as AnimatedSprite;
			SelectedTarget = GetNode("Frame/Selected") as AnimatedSprite;
			Combat = GetNode("Combat") as TextureRect;
			Visualize();
		}

		public void ShowAsValid(bool yes)
		{
			if (yes)
			{
				ValidTarget.Show();
				ValidTarget.Play();
			}
			else
			{
				ValidTarget.Hide();
				ValidTarget.Stop();
			}
		}

		public void ShowAsTargeted()
		{
			SelectedTarget.Show();
			SelectedTarget.Play();
		}

		public void RemoveAura()
		{
			ValidTarget.Hide();
			SelectedTarget.Hide();
			ValidTarget.Stop();
			SelectedTarget.Stop();
		}

		public override string ToString()
		{
			return String.Format("[ {0} : {1} ", Id, Title);
		}

		public void Ready()
		{
			Rest.Visible = false;
		}

		public void Exhaust()
		{
			Rest.Visible = true;
		}

		public bool DoesNotRequireTargets()
		{
			return ValidTargets.Count == 0;
		}

		public void Legalize()
		{
			Legal = true;
			Glow();
		}

		public int C;

		public void SetZ(int value)
		{
			if (C + value < -1)
			{
				return;
			}
			else if (C + value > 1)
			{
				return;
			}

			C += value;
			Frame.ZIndex += value;
		}

		public void Glow()
		{
			if (Legal && GetGlobalRect().HasPoint(GetGlobalMousePosition()) && Zone == Zones.Hand)
			{
				Sound.Play();
				LegalPlay.Visible = true;
				LegalPlay.Play();
				return;
			}

			LegalPlay.Visible = false;
			LegalPlay.Stop();
		}

		public void TurnInvisible()
		{
			Modulate = new Color(Modulate.a, Modulate.g, Modulate.b, 0);
		}

		public void TurnVisible()
		{
			Modulate = new Color(Modulate.a, Modulate.g, Modulate.b, 1);
		}

		public void FlipFaceUp()
		{
			Back.Visible = false;
		}

		public void FlipFaceDown()
		{
			Back.Visible = true;
		}

		public void SetData(BaseCard card)
		{
			Title = card.Title;
			Effect = card.Text;
			Illustration = card.Illustration;
			if (card is Unit unit)
			{
				CardType = unit.CardType;
				Attack = unit.Attack;
				Defense = unit.Defense;
			}
			else if(card is Support support)
			{
				CardType = support.CardType;
				
			}
		}

		public void Visualize()
		{
			if (Blank)
			{
				FlipFaceDown();
				return;
			}

			Identifier.Text = Id.ToString();
			TitleLabel.Text = Title;
			Art.Texture = ResourceLoader.Load(Illustration) as Texture;
			if (CardType == 0)
			{
				// 0 is Unit?
				AttackLabel.Text = Attack.ToString();
				DefenseLabel.Text = Defense.ToString();
			}
			else
			{
				Combat.Hide();
			}

		}

		public void Select()
		{

		}

		[Signal]
		public delegate void CardActivated();

		public void ActivateCard(int skill, Array targets) 
		{
			//Player.Activate([Id])
			//Player.Visual.Animate.Start();
			Legal = false;
			CallDeferred("emit_signal", "activated", this, skill, targets);
		}
		
		private void _on_Card_pressed()
		{
			if (Interact == null)
			{
				return;
			}
			
			if (Interact.Card != null)
			{
				return;
			}

			switch (Zone)
			{
				case Zones.Hand:
					if (CanBeDeployed || CanBeSet)
					{
						Interact.Add(this);
					}
					break;
				case Zones.Unit:
					if (CanAttack)
					{
						Interact.Add(this);
					}
					break;
				case Zones.Support:
					if (CanBeActivated)
					{
						ActivateCard(0, new Array());
					}
					break;
				default:
					return;
			}
		}
		
		
		private void _on_Card_mouse_entered()
		{
			Glow();
		}
		
		
		private void _on_Card_mouse_exited()
		{
		    Sound.Stop();
		    Glow();
		}
		
		
	}
}




