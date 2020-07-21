using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Cards;
using CardGame.Client.Room.Commands;

public class TargetSelection : Panel
{
    private GridContainer SelectionBox;
    private List<Card> Clones = new List<Card>();
    
    public override void _Ready()
    {
        SelectionBox = GetNode<GridContainer>("ScrollContainer/GridContainer");
    }

    public void Reveal(IEnumerable<Card> targets)
    {
        // We want to create copies here, not use the actual cards
        Clear();
        Visible = true;
        foreach (var clone in targets.Select(card => (Card) card.Duplicate((int) (DuplicateFlags.UseInstancing))))
        {
            clone.HighlightAsTarget();
            SelectionBox.AddChild(clone);
        }
        GD.Print("Revealing Targets");
    }

    private void Clear()
    {
        while (SelectionBox.GetChildren().Count > 0)
        {
            SelectionBox.GetChild(0).Free();
        }
    }
}
