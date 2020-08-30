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
		public CardStates State;
		private CardFace _face = CardFace.FaceDown;

		public Sprite3D AttackingIcon;
		public Sprite3D DefendingIcon;
		public IPlayer Player;
		public IZone Zone;
		public readonly IList<Card> ValidTargets = new List<Card>();
		public readonly IList<Card> ValidAttackTargets = new List<Card>();

		// State Checks
		public bool IsInActive = false;
		public bool CanBeDeployed => State == CardStates.CanBeDeployed && Player is Player player && player.State == States.Idle;
		public bool CanBeSet => State == CardStates.CanBeSet && Player is Player player && player.State == States.Idle;
		public bool CanBeActivated => State == CardStates.CanBeActivated && Player is Player player && !player.IsInActive;
		public bool CanAttack => State == CardStates.CanAttack && ValidAttackTargets.Count > 0 && Player is Player player && player.State == States.Idle;

		public bool CanAttackDirectly => State == CardStates.CanAttackDirectly && Player is Player player &&
		                                 player.State == States.Idle;
		public bool CanBePlayed => CanBeSet || CanBeActivated || CanBeDeployed;
		public bool HasAttackTarget(Card card) => ValidAttackTargets.Contains(card);
		

		public Action<Card> MouseOvered;
		public Action<Card> MouseOveredExit;


		public void SetCardArt(Texture art)
		{
			GetNode<Sprite3D>("Face").Texture = art;
		}

		public override void _Ready()
		{
			GetNode<Area>("Area").Connect("mouse_entered", this, nameof(OnMouseEntered));
			GetNode<Area>("Area").Connect("mouse_exited", this, nameof(OnMouseExit));
			AttackingIcon = GetNode<Sprite3D>("Attacking");
			DefendingIcon = GetNode<Sprite3D>("Defending");
			if (CardType == CardType.Unit)
			{
				ChangeAttack();
			}
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
			GetNode<Sprite3D>("Playable").Visible = CanBePlayed;
		}

		private void OnMouseEntered() => MouseOvered(this);
		private void OnMouseExit() => MouseOveredExit(this);

		public void Attack()
		{
			AttackingIcon.Visible = true;
		}

		public void Defend()
		{
			DefendingIcon.Visible = true;
		}
		
		public void DisplayPower(int power)
		{
			throw new System.NotImplementedException();
		}

		public void FlipFaceUp()
		{
			RotationDegrees = new Vector3(Rotation.x, 180, Rotation.z);
		}

		public void FlipFaceDown()
		{
			throw new System.NotImplementedException();
		}
	}
}
