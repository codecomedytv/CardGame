using System;
using Godot;

namespace CardGame.Client.Screens
{
    public class LoginView : Control
    {
        public event Action<string, string> OnLogin;
        public event Action<string, string> OnRegister;
        [Export()] private NodePath Username;
        [Export()] private NodePath Password;
        [Export()] private NodePath LoginButton;
        [Export()] private NodePath RegisterButton;
        private LineEdit UserNameInput;
        private LineEdit PasswordInput;
        private static readonly HttpClient ClientX = new HttpClient();

        public override void _Ready()
        {
            const string pressed = "pressed";
            UserNameInput = GetNode<LineEdit>(Username);
            PasswordInput = GetNode<LineEdit>(Password);
            GetNode<Button>(LoginButton).Connect(pressed, this, nameof(OnLoginPressed));
            GetNode<Button>(RegisterButton).Connect(pressed, this, nameof(OnRegisterPressed));
        }
        
        private async void OnLoginPressed()
        {
            OnLogin?.Invoke(UserNameInput.Text, PasswordInput.Text);   
        }

        private void OnRegisterPressed()
        {
            OnRegister?.Invoke(UserNameInput.Text, PasswordInput.Text);
        }
    
    }

    internal class HttpClient
    {
    }
}
