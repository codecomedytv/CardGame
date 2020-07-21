using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Cards;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Room.View
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class TargetSelection : Panel
	{
		[Signal]
		public delegate void TargetSelected();
		private readonly PackedScene CardCopy = (PackedScene) GD.Load("res://Client/Room/View/CardButton.tscn");
		private GridContainer SelectionBox;
		public CardButton Focused;
		public override void _Ready()
		{
			SelectionBox = GetNode<GridContainer>("ScrollContainer/GridContainer");
		}

		public Array GetTargets()
		{
			return SelectionBox.GetChildren();
		}

		public void Reveal(IEnumerable<Card> targets)
		{
			// We want to create copies here, not use the actual cards
			Clear();
			Visible = true;
			foreach (var card in targets)
			{
				GD.Print($"card id of target is {card.Id}");
				var clone = (CardButton) CardCopy.Instance();
				clone.Connect("pressed", this, nameof(OnPressed), new Array {clone});
				clone.Create(card.Id, card.Art);
				GD.Print($"clone id of target is {card.Id}");
				AddChild(clone);
			}
		}

		private void OnPressed(CardButton clone)
		{
			EmitSignal(nameof(TargetSelected), clone.Id);
		}

		private void Clear()
		{
			while (SelectionBox.GetChildren().Count > 0)
			{
				SelectionBox.GetChild(0).Free();
			}
		}
	}
}
