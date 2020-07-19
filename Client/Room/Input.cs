using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using CardGame.Client.Cards;
using CardGame.Client.Room.View;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Room
{
	public class Input : Godot.Node
	{
		[Signal]
		public delegate void Deploy();

		[Signal]
		public delegate void SetFaceDown();

		[Signal]
		public delegate void Activate();

		[Signal]
		public delegate void Attack();
		
		[Signal]
		public delegate void DirectAttack();

		private readonly Player User;
		private readonly CardCatalog CardCatalog;
		
		public Input(Player player, CardCatalog cardCatalog)
		{
			CardCatalog = cardCatalog;
			User = player;
		}
		
		private IEnumerable<Card> CardsInTree => CardCatalog.Where(c => c.IsInsideTree());

		private object FocusedCard()
		{
			var list = CardsInTree.Where(c => c.GetGlobalRect().HasPoint(c.GetGlobalMousePosition())).ToList();
			return list.Any() ? list[0] : null;
		}

		public override void _Input(InputEvent inputEvent)
		{
			var focusedCard = FocusedCard();
			if (inputEvent is InputEventMouseMotion mouseMove)
			{
				// We don't want to stop highlighting if we're in the act of choosing cards
				if (!User.IsChoosingTargets && !User.IsChoosingAttackTarget)
				{
					foreach (var c in CardsInTree)
					{
						c.StopHighlightingTargets();
						c.StopHighlightingAttackTargets();
						User.Opponent.StopHighlightingAsTarget();
					}
				}

				if (focusedCard is Card viewingCard)
				{
					CardViewer.View(viewingCard);
					if (viewingCard.CanAttack) { viewingCard.HighlightAttackTargets(); }
					if (viewingCard.CanAttackDirectly) {User.Opponent.HighlightAsTarget();}
					if (viewingCard.CanBeActivated) { viewingCard.HighlightTargets(); }
				}
			}
			else if (inputEvent is InputEventMouseButton mouseButton && mouseButton.Doubleclick)
			{
				OnDoubleClick(focusedCard);
			}
		}
		
		public void OnDoubleClick(object focusedCard)
		{
			if (User.IsChoosingAttackTarget)
			{
				if (focusedCard is Card attackTarget)
				{
					ChooseAttackTarget(attackTarget);
				}
				else
				{
					User.CardInUse.Deselect();
					User.CardInUse = null;
					User.Attacking = false;
				}
			}
			else if (User.IsChoosingTargets)
			{
				if (focusedCard is Card effectTarget)
				{
					ChooseEffectTarget(effectTarget);
				}
			}
			else if (!User.IsInActive)
			{
				if (focusedCard is Card card)
				{
					TakeAction(card);
				}
			}
		}
	
		
		private void ChooseEffectTarget(Card card)
		{
			if (!User.CardInUse.HasTarget(card)) return;
			User.CardInUse.StopHighlightingTargets();
			card.ShowAsTargeted();
			EmitSignal(nameof(Activate), User.CardInUse, card.Id);
			User.State = States.Processing;
		}

		private void ChooseAttackTarget(Card card)
		{
			if (User.CardInUse.HasAttackTarget(card))
			{
				User.CardInUse.AttackUnit(card);
				User.State = States.Processing;
				card.ShowAsTargeted();
				EmitSignal(nameof(Attack), User.CardInUse.Id, card.Id);
			}
			else
			{
				User.CardInUse.CancelAttack();
			}

			User.Attacking = false;
			User.CardInUse = null;
		}

		private void TakeAction(Card card) 
		{
			if (card.CanBeDeployed)
			{
				User.State = States.Processing;
				EmitSignal(nameof(Deploy), card.Id);
			}

			else if (card.CanBeSet)
			{
				User.State = States.Processing;
				EmitSignal(nameof(SetFaceDown), card.Id);
			}

			else if (card.CanAttack)
			{
				User.Attacking = true;
				User.CardInUse = card;
				card.Select();
			}
			else if (card.CanAttackDirectly)
			{
				User.Opponent.StopHighlightingAsTarget();
				User.Opponent.ShowAsTargeted();
				card.Select();
				EmitSignal(nameof(DirectAttack), card.Id);
			}

			else if (card.CanBeActivated)
			{
				card.FlipFaceUp();

				// We're checking if it can target, but it seems our fallback (the else) is to activate it without
				// selecting a target which should (in most cases at least) be an illegal play.

				// In retrospect this is okay because if the card required targets but had none valid, it wouldn't
				// be able to satisfy this condition since its state wouldn't be CanBeActivated.
				if (card.CanTarget)
				{
					// We return our of this so players have a chance to select the target before activation
					User.Targeting = true;
					User.CardInUse = card;
					card.HighlightTargets();
				}
				else
				{
					User.State = States.Processing;
					EmitSignal(nameof(Activate), card, new Array());
				} 
			}
		}
	}
}
