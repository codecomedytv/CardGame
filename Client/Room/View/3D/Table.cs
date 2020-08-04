using Godot;
using System;

public class Table : Node
{
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_up"))
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }
}
