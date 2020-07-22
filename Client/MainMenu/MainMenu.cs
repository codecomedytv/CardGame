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
        
        }


    }
}
