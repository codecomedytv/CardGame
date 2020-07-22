using Godot;

namespace CardGame.Client.App
{
    public class SceneManager : Control
    {
        // SceneManager is always root and handles transition between scenes
        
        // Should we manager this via a constructor controller script that handles the scene itself instead of directly?
        private static PackedScene MainMenu = (PackedScene) GD.Load("res://Client/MainMenu/MainMenu.tscn");
        
        private Control CurrentScreen;

        public SceneManager()
        {
        }
        
        public override void _Ready()
        {
            CurrentScreen = GetNode<Control>("Login");
            CurrentScreen.Connect(nameof(Login.LoggedIn), this, nameof(OnLoggedIn));
        }
        
        private void OnLoggedIn()
        {
            var newScreen = MainMenu.Instance();
            RemoveChild(CurrentScreen);
            CurrentScreen.QueueFree();
            CurrentScreen = (Control) newScreen;
            AddChild(newScreen);
        }

        private void OnPlayPressed()
        {
            
        }

        private void OnDeckPressed()
        {
            
        }

        private void OnUserPressed()
        {
            
        }

        private void OnQuitPressed()
        {
            GetTree().Quit();
        }
        
        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent is InputEventKey key && key.IsPressed())
            {
                switch((KeyList) key.Scancode)
                {
                    case KeyList.F: 
                    {
                        OS.WindowFullscreen = !OS.WindowFullscreen;
                        break;
                    }
                    case KeyList.Q: 
                    {
                        GetTree().Quit();
                        break;
                    }
             
                };
            }
        }
    }
}
