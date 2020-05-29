using CardGame.Client.Match;
using CardGame.Server;
using Godot;
using Card = CardGame.Client.Library.Card.Card;

namespace CardGameSharp.Client.Game
{
	public class Manual : GameInput
	{
		public Interact Interact;
		public bool SelectingTarget = false;
		public Card Activated;

		public override void _Process(float delta)
		{
			if (Input.IsActionJustPressed("drag_drop"))
			{
				var mouse = GetGlobalMousePosition();
				if (Interact.Visible)
				{
					Drop(mouse);
				}
			}
		}

		public void Deploy(Card card)
		{
			CallDeferred(nameof(OnDeploy), card);
		}

		public void SetFaceDown(Card card)
		{
			card.Legal = false;
			CallDeferred(nameof(OnSetFaceDown), card);
		}

		public void Attack(Card attacker, Card defender)
		{
			CallDeferred(nameof(OnAttack), attacker, defender.Id);
		}

		public void DirectAttack(Card attacker)
		{
			CallDeferred(nameof(OnAttack), attacker, -1);
		}

		public void PassPriority()
		{
			OnPassPriority();
		}

		public bool Deploying(Vector2 mouse)
		{
			return Player.Visual.Units.GetGlobalRect().HasPoint(mouse) && Interact.Card.CanBeDeployed;
		}

		public bool SettingFaceDown(Vector2 mouse)
		{
			return Player.Visual.Support.GetGlobalRect().HasPoint(mouse) && Interact.Card.CanBeSet;
		}

		public bool Battling(Vector2 mouse)
		{
			return Player.Visual.Units.GetGlobalRect().HasPoint(mouse);
		}

		public bool Attacking(Vector2 mouse)
		{
			return Opponent.Visual.Units.GetGlobalRect().HasPoint(mouse);
		}

		public bool Activating(Vector2 mouse)
		{
			return Player.Visual.Support.GetGlobalRect().HasPoint(mouse);
		}

		public bool Playing(Vector2 mouse)
		{
			return Player.Visual.Hand.GetGlobalRect().HasPoint(mouse);
		}

		public void ShowValidTargets(Card card)
		{
			Activated = card;
			SelectingTarget = true; // State Here (Apparently?)
			foreach (var validTarget in card.ValidTargets)
			{
				Cards[(int)validTarget].ShowAsValid(true);
			}
		}

		public void StopTargeting(Card card)
		{
			foreach (var validTarget in card.ValidTargets)
			{
				Cards[(int)validTarget].ShowAsValid(false);
				card.SelectedTargets.Clear();
				card.ValidTargets.Clear();
				SelectingTarget = false;
				Activated = null;
			}
		}

		public void Drop(Vector2 mouse)
		{
			if (Deploying(mouse))
			{
				Deploy(Interact.Card);
			}
			else if (SettingFaceDown(mouse))
			{
				SetFaceDown(Interact.Card);
			}
			else if (Attacking(mouse))
			{
				Battle(mouse);
			}
			Interact.Stop();
		}

		public void Battle(Vector2 mouse)
		{
			if (!Player.Field.Contains(Interact.Card))
			{
				return;
			}

			var attacker = Interact.Card;
			Card defender = null;

			foreach (var card in Cards.Values)
			{
				if (card.GetGlobalRect().HasPoint(mouse))
				{
					if (Opponent.Field.Contains(card))
					{
						defender = card;
						break;
					}
				}
			}

			if (defender != null && !attacker.ValidTargets.Contains(defender.Id))
			{
				StopTargeting(attacker);
				return;
			}
			if(attacker != null && defender != null)
			{
				attacker.Combat.Show();
				defender.Combat.Show();
				Attack(attacker, defender);
			}

			if (defender == null && Opponent.Field.Count == 0)
			{
				attacker.Combat.Show();
				DirectAttack(attacker);
			}

		}

	}
    
}
