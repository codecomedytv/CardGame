using Godot;
using System;
using System.Collections.Generic;
using CardGame.Client.Cards;
using CardGame.Client.Room.Commands;

public class TargetSelection : Panel
{
    private GridContainer SelectionBox;
    
    public override void _Ready()
    {
        SelectionBox = GetNode<GridContainer>("ScrollContainer/GridContainer");
    }

    public void Reveal(List<Card> targets)
    {
        // We want to create copies here, not use the actual cards
        Visible = true;
        GD.Print("Revealing Targets");
    }
}
