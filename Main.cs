using Godot;
using System;

public class Main : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    	GetNode("Check").Connect("pressed", this, "check");   
    }
	
	public void check(){
		GD.Print(GetNode("VBoxContainer").GetNode("Client").Multiplayer == GetNode("VBoxContainer").GetNode("Client2").Multiplayer);
		GD.Print(GetNode("VBoxContainer").GetNode("Client") == GetNode("VBoxContainer").GetNode("Client2"));
	}

}
