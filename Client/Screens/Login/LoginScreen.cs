using System;
using Godot;

namespace CardGame.Client.Screens
{
    public class LoginScreen: IScreen<LoginView>
    {
        public event Action LoggedIn;
        public LoginView View { get; }

        public LoginScreen()
        {
            View = (LoginView) GD.Load<PackedScene>("res://Client/Screens/Login/Login.tscn").Instance();
            View.OnLogin += OnLoggedIn;
        }
        
        public void Display()
        {
            View.Visible = true;
        }

        public void StopDisplaying()
        {
            View.Visible = false;
        }

        private void OnLoggedIn(string username, string password)
        {
            LoggedIn?.Invoke();
        }

        
    }
}