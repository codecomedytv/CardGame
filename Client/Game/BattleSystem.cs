﻿#nullable enable
using CardGame.Client.Game.Cards;
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

        public void OnAttackStopped(Card attacker, Card? defender = null)
        {
            attacker.RemoveChild(Sword);
            defender?.RemoveChild(Shield);
        }
    }
}