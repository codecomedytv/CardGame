using System;
using Godot;

namespace CardGame.Client.Screens
{
    public class MainMenu: IScreen<Node>
    {
        public event Action FindMatchPressed;
        public event Action DeckEditorPressed;
        public event Action UserProfilePressed;
        public event Action QuitPressed;
        
        public Node View { get; private set; } = new Node();
        
        public void Display()
        {
            throw new NotImplementedException();
        }

        public void StopDisplaying()
        {
            throw new NotImplementedException();
        }

        private void OnFindMatchPressed()
        {
            FindMatchPressed?.Invoke();
        }

        private void OnDeckEditorPressed()
        {
            DeckEditorPressed?.Invoke();
        }

        private void OnUserProfilePressed()
        {
            UserProfilePressed?.Invoke();
        }

        private void OnQuitPressed()
        {
            QuitPressed?.Invoke();
        }
    }
}