using Godot;
using CardGame.Client;

public class Main : Control
{
    public object storedValue;
    public bool IsReady;
    public override void _Ready()
    {
        GetNode<Button>("Play").Connect("pressed", this, "Start");
        StoreValue(false);
        Set(nameof(IsReady), storedValue);
        GD.Print(IsReady);
    }

    public void Start()
    {
        GD.Print("Game Begins");
        GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client").Join();
        GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client2").Join();
    }

    public void StoreValue(object obj) => storedValue = obj;

}
