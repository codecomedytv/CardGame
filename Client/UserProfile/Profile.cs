using Godot;

namespace CardGame.Client.UserProfile
{
    public class Profile: Screen
    {
        [Signal]
        public delegate void BackPressed();
        
        public override void _Ready()
        {
            GetNode<Button>("Back").Connect("pressed", this, nameof(OnBackPressed));
        }

        private void OnBackPressed() => EmitSignal(nameof(BackPressed));
    }
}