using System;
using Godot;

namespace CardGame.Client
{
    public class ScreenManager : Control
    {
        // SceneManager is always root and handles transition between scenes
        
        // Screen Scenes
        // It may be better to seperate the scenes from the controllers?
        private static readonly PackedScene LoginScreen = (PackedScene) GD.Load("res://Client/Login/Login.tscn");
        private static readonly PackedScene MainMenuScreen = (PackedScene) GD.Load("res://Client/MainMenu/MainMenu.tscn");
        
        
        private readonly Login Login = LoginScreen.Instance() as Login;
        private readonly MainMenu MainMenu = MainMenuScreen.Instance() as MainMenu;
        
        private Control CurrentScreen;

        public ScreenManager()
        {
            Subscribe();
            CurrentScreen = Login;
            AddChild(Login);
        }

        private void Subscribe()
        {
            Login.Connect(nameof(Login.LoggedIn), this, nameof(OnLoggedIn));
            MainMenu.Connect(nameof(MainMenu.PlayPressed), this, nameof(OnPlayPressed));
            MainMenu.Connect(nameof(MainMenu.DeckPressed), this, nameof(OnDeckPressed));
            MainMenu.Connect(nameof(MainMenu.UserPressed), this, nameof(OnUserPressed));
            MainMenu.Connect(nameof(MainMenu.QuitPressed), this, nameof(OnQuitPressed));
        }
        
        private void OnLoggedIn()
        {
            RemoveChild(CurrentScreen);
            CurrentScreen = MainMenu;
            AddChild(CurrentScreen);
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
