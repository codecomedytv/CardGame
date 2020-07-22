using Godot;

namespace CardGame.Client
{
    public class MainMenu : Control
    {
        [Signal]
        public delegate void PlayPressed();

        [Signal]
        public delegate void DeckPressed();

        [Signal]
        public delegate void UserPressed();
        
        [Signal]
        public delegate void QuitPressed();
        
        public override void _Ready()
        {
            GetNode<Button>("CenterContainer/VBoxContainer/Play").Connect("pressed", this, nameof(OnPlayPressed));
            GetNode<Button>("CenterContainer/VBoxContainer/Deck").Connect("pressed", this, nameof(OnDeckPressed));
            GetNode<Button>("CenterContainer/VBoxContainer/User").Connect("pressed", this, nameof(OnUserPressed));
            GetNode<Button>("CenterContainer/VBoxContainer/Quit").Connect("pressed", this, nameof(OnQuitPressed));

        }

        private void OnPlayPressed() => EmitSignal(nameof(PlayPressed));
        private void OnDeckPressed() => EmitSignal(nameof(DeckPressed));
        private void OnUserPressed() => EmitSignal(nameof(UserPressed));
        private void OnQuitPressed() => EmitSignal(nameof(QuitPressed));


    }
}
