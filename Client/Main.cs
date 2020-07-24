using Godot;

namespace CardGame.Client
{
    
    public class Main: Control
    {
        private Control ScreenManager;

        public Main()
        {
            ScreenManager = new ScreenManager();
        }
    }
}