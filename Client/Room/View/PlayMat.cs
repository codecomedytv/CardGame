using System;
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

		public override void _Ready()
		{
			Player = GetNode<Player>("Player");
			Opponent = GetNode<Player>("Opponent");
			CardViewer = GetNode<CardViewer>("Background/CardViewer");
			ActionButton = GetNode<Button>("Background/Action");
			DisqualificationNotice = GetNode<Label>("Disqualified");
		}
	
	}
}
