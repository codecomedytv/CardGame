#nullable enable
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
	public class BattleSystem: Object
	{
		private readonly Sprite3D Sword;
		private readonly Sprite3D Shield;

		public BattleSystem()
		{
			Sword = new Sprite3D();
			Shield = new Sprite3D();
			Sword.Texture = GD.Load("res://Client/Assets/HUD/icon_sword.png") as Texture;
			Shield.Texture = GD.Load("res://Client/Assets/HUD/icon_shield.png") as Texture;
			Sword.Name = "Sword";
			Shield.Name = "Shield";
			Sword.Scale /= 2;
			Shield.Scale /= 2;
			Sword.Translation = new Vector3(-0.05F, 0, -0.2F);
			Shield.Translation = new Vector3(-0.05F, 0, -0.2F);
		}
		
		public void OnAttackerSelected(Card attacker)
		{
			attacker.AddChild(Sword);
		}

		public void OnDefenderSelected(Card defender)
		{
			defender.AddChild(Shield);
		}

		public void OnAttackStopped(Card attacker)
		{
			attacker.RemoveChild(Sword);
		}

		public void OnAttackStopped(Card attacker, Card defender)
		{
			attacker.RemoveChild(Sword);
			defender.RemoveChild(Shield);
		}

		public void OnAttackedDirectly(BasePlayer player)
		{
			Shield.Scale *= 2;
			Shield.Translation = player is Player? new Vector3(9.8F, -1, -0.2F): new Vector3(12.5F, 2.7F, 0);
			Shield.RotationDegrees = new Vector3(25, 0, 0);
			player.AddChild(Shield);
		}

		public void OnDirectAttackStopped(BasePlayer player)
		{
			player.RemoveChild(Shield);
			Shield.Scale /= 2;
			Shield.Translation = new Vector3(-0.05F, 0, -0.2F);
			Shield.RotationDegrees = new Vector3(0, 0, 0);
		}
	}
}
