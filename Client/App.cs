using CardGame.Client.Screens;
using Godot;

namespace CardGame.Client
{
    public class App: Node
    {
        private readonly ScreenManager ScreenManager = new ScreenManager();

        public override void _Ready() => AddChild(ScreenManager);
    }
}