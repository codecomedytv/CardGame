using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Cards
{
	public class Card: Spatial
	{
		public int Id;
		public string Title;
		public int Power;
		public CardType CardType;
		public string Effect;
		public Texture Art;
		public readonly Targets ValidTargets = new Targets();
		public readonly Targets ValidAttackTargets = new Targets();
		private Sprite3D TargetReticule;
		public int ZoneIndex = -1;
		public Zone Zone;

		public CardStates State => CardState.State;
		private readonly CardState CardState;

		private Area Area;
		private Sprite3D AttackIcon;
		private Sprite3D DefenseIcon;
		private AnimationPlayer AnimationPlayer;

		public bool IsHidden { get; set; } = true;
		public Player OwningPlayer { get; set; }
		public Player Controller { get; set; }
		
		public bool HasAttackTarget(Card card) => ValidAttackTargets.Contains(card);
		public Action<Card> MouseOvered;
		public Action<Card> MouseOveredExit;

		public Card()
		{
			CardState = new CardState(this);
		}
		
		public override void _Ready()
		{
			TargetReticule = GetNode<Sprite3D>("Target");
			Area = GetNode<Area>("Area");
			Area.Connect("mouse_entered", this, nameof(OnMouseEntered));
			Area.Connect("mouse_exited", this, nameof(OnMouseExit));
			AttackIcon = GetNode<Sprite3D>("Attacking");
			DefenseIcon = GetNode<Sprite3D>("Defending");
			AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			if (CardType == CardType.Unit)
			{
				ChangeAttack();
			}
		}

		public void Update(CardStates state, IList<Card> targets, IList<Card> attackTargets)
		{
			CardState.State = state;
			ValidTargets.Update(targets);
			ValidAttackTargets.Update(attackTargets);
		}

		public void SetCardArt(Texture art)
		{
			Art = art;
			GetNode<Sprite3D>("Face").Texture = Art;
		}

		
		public void Target()
		{
			TargetReticule.Visible = true;
			AnimationPlayer.Play("Target");
		}

		public void StopTargeting()
		{
			TargetReticule.Visible = false;
			AnimationPlayer.Play("Playable");
		}

		public void Attack()
		{
			AttackIcon.Visible = true;
		}

		public void StopAttack()
		{
			AttackIcon.Visible = false;
		}

		public void Defend()
		{
			DefenseIcon.Visible = true;
		}

		public void StopDefend()
		{
			DefenseIcon.Visible = false;
		}
		
		private void ChangeAttack()
		{
			var i = 0;
			foreach (var element in Power.ToString())
			{
				GetNode<Sprite3D>($"Power/{i}").Texture = GD.Load($"res://Client/Assets/Numbers/{element}.png") as Texture;
				GetNode<Sprite3D>($"Power/{i}").Visible = true;
				i += 1;
			}

			GetNode<Spatial>("Power").Visible = true;
		}

		public void OnPlayerStateChanged(States state)
		{
			GetNode<Sprite3D>("Playable").Visible = CardState.CanBePlayed();
		}

		private void OnMouseEntered()
		{
			if (!IsHidden)
			{
				MouseOvered(this);
			}
		}

		private void OnMouseExit()
		{
			if (!IsHidden)
			{
				MouseOveredExit(this);
			}
		}
		
		public void FlipFaceUp()
		{
			RotationDegrees = new Vector3(Rotation.x, 180, Rotation.z);
		}
	}
}
