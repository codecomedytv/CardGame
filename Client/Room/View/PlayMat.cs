using System;
using System.Security.Policy;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Room.View
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class PlayMat: Sprite
	{
		public Player Player;
		public Player Opponent;
		public CardViewer CardViewer;
		public Button ActionButton;
		public Label DisqualificationNotice;
		public Label WinLoseNotice;
		public TargetSelection TargetSelection;

		public override void _Ready()
		{
			Player = GetNode<Player>("Player");
			Opponent = GetNode<Player>("Opponent");
			CardViewer = GetNode<CardViewer>("Background/CardViewer");
			ActionButton = GetNode<Button>("Background/Action");
			DisqualificationNotice = GetNode<Label>("Disqualified");
			WinLoseNotice = GetNode<Label>("WinLoseNotice");
			TargetSelection = GetNode<TargetSelection>("TargetSelection");
		}
	
	}
}
