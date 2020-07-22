using System;
using Godot;

namespace CardGame.Client.App
{
    public class SceneManager : Control
    {
        // SceneManager is always root and handles transition between scenes
        
        // Should we manager this via a constructor controller script that handles the scene itself instead of directly?
        private static PackedScene MainMenuScreen = (PackedScene) GD.Load("res://Client/MainMenu/MainMenu.tscn");
        
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
            var newScreen = MainMenuScreen.Instance();
            RemoveChild(CurrentScreen);
            CurrentScreen.QueueFree();
            CurrentScreen = (MainMenu) newScreen;
            AddChild(newScreen);
            newScreen.Connect(nameof(MainMenu.PlayPressed), this, nameof(OnPlayPressed));
            newScreen.Connect(nameof(MainMenu.DeckPressed), this, nameof(OnDeckPressed));
            newScreen.Connect(nameof(MainMenu.UserPressed), this, nameof(OnUserPressed));
            newScreen.Connect(nameof(MainMenu.QuitPressed), this, nameof(OnQuitPressed));
        }

        private void OnPlayPressed()
        {
            throw new NotImplementedException();
        }

        private void OnDeckPressed()
        {
            throw new NotImplementedException();

        }

        private void OnUserPressed()
        {
            throw new NotImplementedException();
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
