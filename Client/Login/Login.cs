using Godot;

namespace CardGame.Client
{
    public class Login : Control
    {
        // This will exist as a dummy login for the time being
        // In future iterations we will use HTTPRequests to communicate with
        // an online user database

        private LineEdit UserName;
        private LineEdit Password;

        public override void _Ready()
        {
            UserName = GetNode<LineEdit>("VBoxContainer/UserName");
            Password = GetNode<LineEdit>("VBoxContainer/Password");
            GetNode<Button>("VBoxContainer/Login").Connect("pressed", this, nameof(OnLoginPressed));
        }

        private void OnLoginPressed()
        {
            
        }
        
    }
}
